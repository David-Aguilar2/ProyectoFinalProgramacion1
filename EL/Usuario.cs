using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EL
{
    // Clase que representa la tabla de Usuarios en la base de datos
    public class Usuario
    {
        // Definición de constantes para identificar los niveles de acceso
        public const int ROL_SUPERADMIN = 1;
        public const int ROL_ADMIN = 2;
        public const int ROL_TRABAJADOR = 3;

        // Clave primaria que se incrementa automáticamente en la base de datos
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        // Nombre completo obligatorio con un máximo de 100 caracteres
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        // Correo electrónico con formato válido y que no permite duplicados en el sistema
        [StringLength(120)]
        [EmailAddress]
        [Index(IsUnique = true)]
        public string Correo { get; set; }

        // Nombre de usuario único para el inicio de sesión
        [Required, StringLength(50)]
        public string Username { get; set; }

        // Contraseña almacenada (normalmente de forma encriptada)
        [Required, StringLength(100)]
        public string ClaveAcceso { get; set; }

        // Número de contacto obligatorio
        [Required, StringLength(30)]
        public string Telefono { get; set; }

        // Dirección física del usuario
        [Required, StringLength(200)]
        public string Direccion { get; set; }

        // Identificador del rol asignado (1, 2 o 3)
        [Required]
        public int Rol { get; set; }

        // Indica si el usuario puede entrar al sistema (Activo/Inactivo)
        [Required]
        public bool Estado { get; set; }

        // Constructor: valores iniciales al crear un nuevo usuario
        public Usuario()
        {
            Estado = true; // Por defecto está activo
            Rol = ROL_TRABAJADOR; // Por defecto inicia con el rango más bajo
        }
    }
}