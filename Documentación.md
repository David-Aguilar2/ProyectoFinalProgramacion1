# Documentación Técnica - IconicFashion

## Arquitectura del Proyecto

IconicFashion está diseñada con un patrón de capas clásico: `GUI`, `BLL`, `DAL` y `EL`.

- `EL` (Entidades): define el modelo de datos y las reglas de esquema para Entity Framework.
- `DAL` (Data Access Layer): encapsula el acceso a la base de datos mediante `IconicFashionDbContext` y clases CRUD.
- `BLL` (Business Logic Layer): contiene reglas de negocio, validaciones y transacciones.
- `GUI` (Interfaz Gráfica): expone los formularios WinForms y consume los servicios de BLL.

La interacción entre capas es unidireccional:

- `GUI` usa `BLL`.
- `BLL` usa `DAL` y `EL`.
- `DAL` usa `EL` y `IconicFashionDbContext`.

Este patrón separa responsabilidades, hace más fácil testear la lógica de negocio y evita que la UI acceda directamente a la base de datos.

---

## Capa EL (Entidades)

### Usuario

La clase `EL.Usuario` representa al actor del sistema con validación de esquema.

```csharp
public class Usuario
{
    public const int ROL_SUPERADMIN = 1;
    public const int ROL_ADMIN = 2;
    public const int ROL_TRABAJADOR = 3;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUsuario { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(120)]
    [EmailAddress]
    [Index(IsUnique = true)]
    public string Correo { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; }

    [Required, StringLength(100)]
    public string ClaveAcceso { get; set; }

    [Required, StringLength(30)]
    public string Telefono { get; set; }

    [Required, StringLength(200)]
    public string Direccion { get; set; }

    [Required]
    public int Rol { get; set; }

    [Required]
    public bool Estado { get; set; }
}
```

- `Key`: marca `IdUsuario` como clave primaria.
- `DatabaseGenerated(DatabaseGeneratedOption.Identity)`: indica que la base de datos genera el valor.
- `Required`: obliga que la propiedad tenga valor.
- `StringLength(...)`: limita la longitud del texto.
- `EmailAddress`: valida el formato de correo.
- `Index(IsUnique = true)`: asegura unicidad en la columna `Correo`.

> Advertencia técnica: `Index(IsUnique = true)` es un atributo de Entity Framework 6 y crea un índice único en el esquema, garantizando que no existan dos usuarios con el mismo correo.

### Producto

```csharp
[Table("Productos")]
public class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdProducto { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, StringLength(200)]
    public string Descripcion { get; set; }

    [Required]
    [Column(TypeName = "decimal")]
    public decimal Precio { get; set; }

    [Required]
    public bool Estado { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [ForeignKey("IdCategoria")]
    public virtual Categoria Categoria { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public DateTime FechaRegistro { get; set; }

    public bool StockBajo()
    {
        return Cantidad <= 4;
    }
}
```

- `ForeignKey("IdCategoria")`: relaciona `Producto` con `Categoria`.
- `StockBajo()`: helper para detectar inventario crítico.

### Categoria

```csharp
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
```

### RegistroSalida

```csharp
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
    public string Tipo { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public string Motivo { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime FechaSalida { get; set; }
}
```

- `Tipo`: distingue entre `ENTRADA` y `SALIDA`.
- `FechaSalida`: se mapea como `datetime2` para mayor precisión.

---

## Capa DAL (Acceso a Datos)

### IconicFashionDbContext

El contexto `DAL.IconicFashionDbContext` es el punto central de Entity Framework:

```csharp
public class IconicFashionDbContext : DbContext
{
    public IconicFashionDbContext() : base("name=IconicFashionDbContext")
    {
        Database.SetInitializer<IconicFashionDbContext>(null);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<EL.Usuario> Usuarios { get; set; }
    public virtual DbSet<EL.Categoria> Categorias { get; set; }
    public virtual DbSet<EL.Producto> Productos { get; set; }
    public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
}
```

- `Database.SetInitializer<IconicFashionDbContext>(null)`: deshabilita los inicializadores automáticos de EF.
  - Esto evita que Entity Framework intente crear o modificar el esquema automáticamente.
  - Es una decisión útil en entornos donde las migraciones se aplican manualmente o la base de datos ya existe.
- `modelBuilder.Conventions.Remove<IncludeMetadataConvention>()`: elimina la convención que busca la tabla `__MigrationHistory`.
  - Se usa para impedir que EF intente validar metadatos de migraciones cuando la base de datos puede no incluir dicha tabla, o cuando se usa un control manual de esquema.

### Patrón CRUD en DAL

Las clases DAL siguen un patrón CRUD sencillo:

- `BuscarPorId` / `ObtenerProducto` / `ObtenerUsuarios` / `ObtenerProductos`
- `Guardar` con `EntityState.Modified` para actualizar o `Add` para insertar.
- `Eliminar` usa `Find` y `Remove`.

Ejemplo de guardado genérico en `DAL.UsuarioDAL`:

```csharp
public int Guardar(Usuario usuario)
{
    _db = new IconicFashionDbContext();

    if (usuario.IdUsuario > 0)
    {
        _db.Entry(usuario).State = EntityState.Modified;
    }
    else
    {
        _db.Usuarios.Add(usuario);
    }

    _db.SaveChanges();
    return usuario.IdUsuario;
}
```

### Método `ObtenerRegistrosSalida` y carga ansiosa

El método más relevante en `DAL.RegistroSalidaDAL` es:

```csharp
public List<RegistroSalida> ObtenerRegistrosSalida()
{
    using (_db = new IconicFashionDbContext())
    {
        return _db.RegistrosSalida
                  .Include(r => r.Producto)
                  .Include(r => r.Usuario)
                  .ToList();
    }
}
```

- `.Include(r => r.Producto)` y `.Include(r => r.Usuario)` realizan carga ansiosa (eager loading).
- Esto permite que los datos relacionados de producto y usuario estén disponibles inmediatamente.
- Evita `LazyLoading` y múltiples consultas posteriores cuando la UI necesita mostrar datos de las entidades relacionadas.

---