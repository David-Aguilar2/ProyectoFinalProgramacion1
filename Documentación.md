public class Usuario
{
    // Constantes para la gestión de Roles (Niveles de Acceso)
    public const int ROL_SUPERADMIN = 1;
    public const int ROL_ADMIN = 2;
    public const int ROL_TRABAJADOR = 3;

    [Key] // Clave Primaria
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental en SQL
    public int IdUsuario { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(120), EmailAddress] // Validación de formato de correo
    public string Correo { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; }

    [Required, StringLength(100)]
    public string ClaveAcceso { get; set; }

    [Required]
    public int Rol { get; set; } // Almacena el valor de las constantes superiores

    [Required]
    public bool Estado { get; set; } // Para borrado lógico

    public Usuario()
    {
        Estado = true; // Por defecto activo
        Rol = ROL_TRABAJADOR; // Rol inicial base
    }
}

public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCategoria { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, StringLength(200)]
    public string Descripcion { get; set; }
}

[Table("Productos")] // Mapeo explícito al nombre de la tabla en SQL
public class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdProducto { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, Column(TypeName = "decimal")] // Asegura precisión de dos decimales
    public decimal Precio { get; set; }

    [Required]
    public int Cantidad { get; set; } // Stock actual

    [Required]
    public int IdCategoria { get; set; }

    [ForeignKey("IdCategoria")] // Relación de integridad referencial
    public virtual Categoria Categoria { get; set; }

    [Required]
    public bool Estado { get; set; }

    public Producto() { Estado = true; }

    // Lógica de Negocio en la Entidad:
    public bool StockBajo()
    {
        return Cantidad <= 4; // Umbral de alerta para reposición
    }
}

public class RegistroSalida
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdRegistro { get; set; }

    [Required]
    public int IdProducto { get; set; }
    [ForeignKey("IdProducto")]
    public virtual Producto Producto { get; set; }

    [Required]
    public int IdUsuario { get; set; }
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; }

    [Required]
    public string Tipo { get; set; } // "ENTRADA" o "SALIDA"

    [Required]
    public int Cantidad { get; set; } // Magnitud del movimiento

    [Required]
    public string Motivo { get; set; } // Justificación del cambio

    [Required]
    [Column(TypeName = "datetime2")] // Soporta rangos de fecha más amplios y precisos
    public DateTime FechaSalida { get; set; }

    public RegistroSalida()
    {
        FechaSalida = DateTime.Now; // Registro automático de marca temporal
    }
}

Capa DAL (Data Access Layer)
Esta capa es la única responsable de comunicarse con SQL Server. Utiliza Entity Framework 6 y sigue un patrón de acceso directo a través de clases especializadas por entidad.

Componentes
- IconicFashionDbContext
Es el coordinador principal de Entity Framework. Gestiona las conexiones y el mapeo de objetos a tablas.


public class IconicFashionDbContext : DbContext
{
    // Constructor: Vincula el código con la cadena de conexión del App.config
    public IconicFashionDbContext() : base("name=IconicFashionDbContext")
    {
        // Database.SetInitializer(null): Desactiva la creación automática de tablas.
        // Esto es vital para evitar errores si la base de datos ya existe o si 
        // hay discrepancias con el historial de migraciones de Entity Framework.
        Database.SetInitializer<IconicFashionDbContext>(null);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // modelBuilder.Conventions.Remove: Elimina la convención que incluye metadatos.
        // Evita que el sistema busque la tabla técnica '__MigrationHistory', 
        // mejorando la velocidad de inicio y evitando excepciones de permisos.
        modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
        base.OnModelCreating(modelBuilder);
    }

    // DbSet<T>: Representan las tablas físicas. Cada propiedad permite realizar 
    // consultas LINQ que EF traduce automáticamente a lenguaje SQL.
    public virtual DbSet<EL.Usuario> Usuarios { get; set; }
    public virtual DbSet<EL.Categoria> Categorias { get; set; }
    public virtual DbSet<EL.Producto> Productos { get; set; }
    public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
}

-Métodos CRUD Estándar (UsuarioDAL, CategoriaDAL, ProductoDAL)
public class ProductoDAL 
{
    IconicFashionDbContext _db;

    // ObtenerProducto: Recupera un único registro.
    // El método .Find(id) es el más eficiente porque busca primero en la 
    // memoria local antes de consultar al servidor SQL.
    public Producto ObtenerProducto(int id)
    {
        _db = new IconicFashionDbContext();
        return _db.Productos.Find(id); 
    }

    // Guardar: Método polivalente (Upsert).
    // Si IdProducto > 0: Usa .Entry().State = Modified para generar un "UPDATE" en SQL.
    // Si IdProducto == 0: Usa .Add() para generar un "INSERT INTO".
    // .SaveChanges() ejecuta físicamente la instrucción en la base de datos.
    public int Guardar(Producto producto)
    {
        _db = new IconicFashionDbContext();
        if (producto.IdProducto > 0) 
            _db.Entry(producto).State = EntityState.Modified;
        else 
            _db.Productos.Add(producto);

        _db.SaveChanges();
        return producto.IdProducto;
    }

    // ObtenerProductos: Carga masiva de datos.
    // .ToList() fuerza la ejecución de la consulta SELECT * FROM Productos.
    public List<Producto> ObtenerProductos()
    {
        _db = new IconicFashionDbContext();
        return _db.Productos.ToList(); 
    }

    // Eliminar: Borrado físico.
    // Primero localiza el objeto y luego lo marca para su eliminación definitiva.
    public int Eliminar(int id)
    {
        _db = new IconicFashionDbContext();
        Producto obj = _db.Productos.Find(id);
        if (obj != null) {
            _db.Productos.Remove(obj);
            _db.SaveChanges();
        }
        return id;
    }
}

-Operaciones Especializadas: RegistroSalidaDAL

Esta clase es diferente a las demás por dos razones críticas:

Gestión de Memoria: Utiliza bloques using para asegurar que la conexión se cierre inmediatamente, evitando conflictos de rastreo de entidades.

Carga Relacionada (Joins): Utiliza .Include() para traer los nombres de productos y usuarios en una sola consulta, evitando que la grid muestre celdas vacías o "N/A".

public class RegistroSalidaDAL
{
    private IconicFashionDbContext _db;

    public int Guardar(RegistroSalida registroSalida)
    {
        // Bloque using: Garantiza que la conexión se cierre y se libere de 
        // memoria (Dispose) al terminar, incluso si ocurre un error. 
        // Esto previene fugas de memoria y bloqueos en la base de datos.
        using (_db = new IconicFashionDbContext())
        {
            if (registroSalida.IdRegistro == 0)
                _db.RegistrosSalida.Add(registroSalida);
            else
                _db.Entry(registroSalida).State = EntityState.Modified;

            _db.SaveChanges();
            return registroSalida.IdRegistro;
        }
    }

    public List<RegistroSalida> ObtenerRegistrosSalida()
    {
        using (_db = new IconicFashionDbContext())
        {
            // Eager Loading (.Include): Por defecto, EF no carga datos de otras tablas.
            // .Include(r => r.Producto) fuerza un "INNER JOIN" en SQL para traer el 
            // nombre del producto y del usuario en una sola transacción. 
            // Sin esto, veríamos IDs o valores nulos en la interfaz de usuario.
            return _db.RegistrosSalida
                      .Include(r => r.Producto)
                      .Include(r => r.Usuario)
                      .ToList();
        }
    }
}


    










