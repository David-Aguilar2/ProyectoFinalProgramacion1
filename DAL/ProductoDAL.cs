using EL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ProductoDAL
    {
        public void InsertarProducto(Producto producto)
        {
            using (var context = new IconicFashionContext())
            {
                context.Productos.Add(producto);
                context.SaveChanges();
            }
        }

        public List<Producto> ObtenerProductos()
        {
            using (var context = new IconicFashionContext())
            {
                return context.Productos.ToList();
            }
        }

        public void ActualizarProducto(Producto producto)
        {
            using (var context = new IconicFashionContext())
            {
                context.Productos.Update(producto);
                context.SaveChanges();
            }
        }

        public void EliminarProducto(int idProducto)
        {
            using (var context = new IconicFashionContext())
            {
                var producto = context.Productos.Find(idProducto);
                if (producto != null)
                {
                    context.Productos.Remove(producto);
                    context.SaveChanges();
                }
            }
        }
    }
}
