using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public int Cantidad { get; set; }

        public bool StockBajo()
        {
            return Cantidad <= 4;
        }
    }
}
