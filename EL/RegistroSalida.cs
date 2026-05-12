using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
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

        public RegistroSalida()
        {
            FechaSalida = DateTime.Now;
        }

    }

}
