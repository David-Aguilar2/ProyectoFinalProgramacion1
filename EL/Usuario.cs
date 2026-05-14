using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
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

        public Usuario()
        {
            Estado = true;
            Rol = ROL_TRABAJADOR;
        }
    }
}
