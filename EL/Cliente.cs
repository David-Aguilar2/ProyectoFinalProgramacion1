using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }

        public Usuario Usuario { get; set; }

        public Cliente()
        {
            
        }

        public Cliente(int id, string nombre, string correo, string telefono, string direccion, 
                       bool estado, DateTime fechaRegistro, DateTime? fechaModificacion,
                       string usuarioRegistro, string usuarioModificacion, Usuario user)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Telefono = telefono;
            Direccion = direccion;
            Estado = estado;
            FechaRegistro = fechaRegistro;
            FechaModificacion = fechaModificacion;
            UsuarioRegistro = usuarioRegistro;
            UsuarioModificacion = usuarioModificacion;
            Usuario = user;
        }
    }
}
