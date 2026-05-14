using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
    // Clase que representa el historial de movimientos (Entradas y Salidas) de inventario
    public class RegistroSalida
    {
        // Clave primaria de la tabla que se genera automáticamente al crear un registro
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRegistro { get; set; }

        // Identificador del producto relacionado con este movimiento
        [Required]
        public int IdProducto { get; set; }

        // Propiedad que permite acceder a toda la información del producto vinculado
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }

        // Identificador del usuario que realizó la operación
        [Required]
        public int IdUsuario { get; set; }

        // Propiedad que permite acceder a los datos del usuario que registró el movimiento
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        // Indica si el movimiento es una "ENTRADA" o una "SALIDA" de mercancía
        [Required]
        public string Tipo { get; set; }

        // Cantidad de unidades que entraron o salieron del inventario
        [Required]
        public int Cantidad { get; set; }

        // Explicación breve del porqué se realizó el movimiento (ej. Venta, Ajuste)
        [Required]
        public string Motivo { get; set; }

        // Fecha y hora exacta del movimiento; usa datetime2 para mayor precisión en SQL
        [Required]
        public DateTime FechaSalida { get; set; }

        // Constructor: asigna automáticamente la fecha y hora actual al crear el registro
        public RegistroSalida()
        {
            FechaSalida = DateTime.Now;
        }
    }
}