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
