using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
    // Clase que representa la tabla de Categorías de productos en la base de datos
    public class Categoria
    {
        // Clave primaria de la tabla; el valor se genera automáticamente al insertar un registro
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        // Nombre de la categoría; es obligatorio y tiene un límite de 100 caracteres
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        // Detalle de la categoría; es obligatorio y tiene un límite de 200 caracteres
        [Required, StringLength(200)]
        public string Descripcion { get; set; }
    }
}