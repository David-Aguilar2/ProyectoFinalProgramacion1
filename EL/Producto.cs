using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
    // Clase que representa la tabla de Productos en la base de datos
    [Table("Productos")]
    public class Producto
    {
        // Clave primaria de la tabla con incremento automático
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }

        // Nombre del artículo; es obligatorio y tiene un límite de 100 caracteres
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        // Breve detalle del producto; obligatorio con límite de 200 caracteres
        [Required, StringLength(200)]
        public string Descripcion { get; set; }

        // Valor monetario definido como tipo decimal para mayor precisión financiera
        [Required]
        [Column(TypeName = "decimal")]
        public decimal Precio { get; set; }

        // Indica si el producto está disponible para la venta (Activo/Inactivo)
        [Required]
        public bool Estado { get; set; }

        // Relación numérica con la categoría a la que pertenece el producto
        [Required]
        public int IdCategoria { get; set; }

        // Propiedad de navegación que permite acceder a los datos de la Categoría vinculada
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

        // Cantidad de existencias físicas disponibles en el inventario
        [Required]
        public int Cantidad { get; set; }

        // Almacena el día y la hora en que se dio de alta el producto
        [Required]
        public DateTime FechaRegistro { get; set; }

        // Constructor: establece que por defecto un producto nuevo esté activo
        public Producto()
        {
            Estado = true;
        }

        // Función que devuelve verdadero si las existencias son 4 unidades o menos
        public bool StockBajo()
        {
            return Cantidad <= 4;
        }
    }
}