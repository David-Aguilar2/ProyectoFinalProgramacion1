using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Username { get; set; }
        public string ClaveAcceso { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }

        public Usuario()
        {
            // Constructor vacío
        }
        public Usuario(int id, string nombre, string correo, string username, string claveAcceso, 
            string telefono, string direccion, bool estado)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Username = username;
            ClaveAcceso = claveAcceso;
            Telefono = telefono;
            Direccion = direccion;
            Estado = estado;
        }
    }
}
