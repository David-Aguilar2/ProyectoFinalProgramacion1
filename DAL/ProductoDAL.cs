using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    // Esta clase se encarga de las operaciones reales en la tabla de Productos
    public class ProductoDAL
    {
        // Conexión principal a la base de datos
        IconicFashionDbContext _db;

        // Busca un producto específico usando su número de ID
        public Producto ObtenerProducto(int id)
        {
            _db = new IconicFashionDbContext();
            return _db.Productos.Find(id);
        }

        // Este método sirve para guardar un producto nuevo o actualizar los datos de uno que ya existe
        public int Guardar(Producto producto)
        {
            _db = new IconicFashionDbContext();

            // Si el producto ya tiene un ID mayor a 0, el sistema entiende que solo debe modificarlo
            if (producto.IdProducto > 0)
            {
                _db.Entry(producto).State = EntityState.Modified;
            }
            // Si el ID es 0, es un producto nuevo y se agrega a la lista de la base de datos
            else
            {
                _db.Productos.Add(producto);
            }

            // Confirma y guarda los cambios realizados
            _db.SaveChanges();
            return producto.IdProducto;
        }

        // Trae la lista de todos los productos almacenados en el sistema
        public List<Producto> ObtenerProductos()
        {
            _db = new IconicFashionDbContext();
            return _db.Productos.ToList();
        }

        // Elimina un producto permanentemente usando su número de ID
        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            // Primero buscamos si el producto realmente existe en la base de datos
            Producto producto = _db.Productos.Find(id);

            // Si no lo encuentra, no hace nada y devuelve 0
            if (producto == null)
                return resultado;

            // Si lo encuentra, lo borra y guarda el cambio definitivamente
            _db.Productos.Remove(producto);
            _db.SaveChanges();

            resultado = producto.IdProducto;

            return resultado;
        }
    }
}