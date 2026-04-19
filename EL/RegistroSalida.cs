using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class RegistroSalida
    {
        public int IdRegistro { get; set; }
        public string IdProducto { get; set; }
        public string NombreCliente { get; set; }
        public decimal TelefonoCliente { get; set; }
        public string Cantidad { get; set; }
        public DateTime FechaSalida { get; set; }
    }
}
