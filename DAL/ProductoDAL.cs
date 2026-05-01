using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    public class ProductoDAL
    {
        IconicFashionDbContext _db;

        public Producto ObtenerProducto(int id)
        {
            _db = new IconicFashionDbContext();
            return _db.Productos.Find(id);
        }

        public int Guardar(Producto producto)
        {
            _db = new IconicFashionDbContext();

            if (producto.IdProducto > 0)
            {
                _db.Entry(producto).State = EntityState.Modified;
            }
            else
            {
                _db.Productos.Add(producto);
            }

            _db.SaveChanges();
            return producto.IdProducto;
        }

        public List<Producto> ObtenerProductos()
        {
            _db = new IconicFashionDbContext();
            return _db.Productos.ToList();
        }

        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            Producto producto = _db.Productos.Find(id);

            if (producto == null)
                return resultado;

            _db.Productos.Remove(producto);
            _db.SaveChanges();

            resultado = producto.IdProducto;

            return resultado;
        }
    }
}