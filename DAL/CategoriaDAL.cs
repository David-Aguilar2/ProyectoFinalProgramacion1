using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    // Esta clase se encarga de las operaciones reales en la tabla de Categorías
    public class CategoriaDAL
    {
        // Conexión a la base de datos
        IconicFashionDbContext _db;

        // Busca una categoría específica usando su número de ID
        public Categoria ObtenerCategoria(int id)
        {
            _db = new IconicFashionDbContext();
            return _db.Categorias.Find(id);
        }

        // Este método sirve para registrar una nueva categoría o actualizar una existente
        public int Guardar(Categoria categoria, int id = 0, bool esEdicion = false)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            // Si el sistema nos indica que es una edición (cambio de datos)
            if (esEdicion)
            {
                categoria.IdCategoria = id;

                // Le decimos a la base de datos que este registro ya existe y ha sido modificado
                _db.Entry(categoria).State = EntityState.Modified;
                _db.SaveChanges();
            }
            // Si no es edición, significa que es una categoría nueva para la lista
            else
            {
                _db.Categorias.Add(categoria);
                _db.SaveChanges();
            }

            resultado = categoria.IdCategoria;
            return resultado;
        }

        // Trae la lista de todas las categorías guardadas hasta el momento
        public List<Categoria> ObtenerCategorias()
        {
            _db = new IconicFashionDbContext();
            return _db.Categorias.ToList();
        }

        // Borra una categoría de forma permanente usando su ID
        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            // Buscamos si la categoría existe antes de intentar borrarla
            Categoria categoria = _db.Categorias.Find(id);

            // Si no la encuentra, se detiene y devuelve 0
            if (categoria == null)
                return resultado;

            // Si la encuentra, la elimina de la tabla y guarda los cambios
            _db.Categorias.Remove(categoria);
            _db.SaveChanges();

            resultado = categoria.IdCategoria;

            return resultado;
        }
    }
}