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
        public string Username { get; set; }
        public string ClaveAcceso { get; set; }

        public int IdCliente { get; set; }

        public Usuario()
        {
            // Constructor vacío
        }
        public Usuario(int id, string username, string claveAcceso, int idCliente )
        {
            Id = id;
            Username = username;
            ClaveAcceso = claveAcceso;
            IdCliente = idCliente;
        }
    }
}
