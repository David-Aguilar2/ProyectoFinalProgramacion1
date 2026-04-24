using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    public class CategoriaDAL
    {
        IconicFashionDbContext _db;

        public int Guardar(Categoria categoria, int id = 0, bool esEdicion = false)
        {
            int resultado = 0;

            _db = new IconicFashionDbContext();

            if (esEdicion)
            {
                categoria.IdCategoria = id;

                _db.Entry(categoria).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                _db.Categorias.Add(categoria);
                _db.SaveChanges();
            }

            resultado = categoria.IdCategoria;
            return resultado;
        }

        public List<Categoria> ObtenerCategorias()
        {
            _db = new IconicFashionDbContext();
            return _db.Categorias.ToList();
        }

        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            Categoria categoria = _db.Categorias.Find(id);

            if (categoria == null)
                return resultado;

            _db.Categorias.Remove(categoria);
            _db.SaveChanges();

            resultado = categoria.IdCategoria;

            return resultado;
        }
    }
}
