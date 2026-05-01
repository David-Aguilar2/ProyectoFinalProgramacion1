using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CategoriaBLL
    {
        public CategoriaBLL() { }
        public CategoriaDAL categoriaDAL  = new CategoriaDAL();

        //Buscar por ID
        public Categoria ObtenerCategoriaPorId(int id)
        {
            return categoriaDAL.ObtenerCategoria(id);
        }

        public string InsertarCategoria(Categoria categoria)
        {
            // Validar que la categoría no sea nula
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                return "El nombre de la categoría es obligatorio";

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                return "La descripción de la categoría es obligatoria";

            // Validar duplicados
            var lista = categoriaDAL.ObtenerCategorias();
            if (lista.Any(c => c.Nombre.ToLower() == categoria.Nombre.ToLower()))
                return "Ya existe una categoría con ese nombre";

            // Guardar en base de datos
            categoriaDAL.Guardar(categoria);

            return "OK";
        }
        public string ActualizarCategoria(Categoria categoria)
        {
            if (categoria == null)
                return "Error: categoría vacía";

            if (categoria.IdCategoria <= 0)
                return "Categoría inválida";

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                return "El nombre de la categoría es obligatorio";

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                return "La descripción de la categoría es obligatoria";

            // Validar duplicados (excepto la misma categoría que se está actualizando)
            var lista = categoriaDAL.ObtenerCategorias();
            if (lista.Any(c => c.Nombre.ToLower() == categoria.Nombre.ToLower()
                               && c.IdCategoria != categoria.IdCategoria))
                return "Ya existe otra categoría con ese nombre";

            categoriaDAL.Guardar(categoria, categoria.IdCategoria, true);

            return "OK";
        }


        public List<Categoria> ObtenerCategorias()
        {
            return categoriaDAL.ObtenerCategorias();
        }
        public Categoria ObtenerCategoria(int id)
        {
            return categoriaDAL.ObtenerCategoria(id);
        }

        public string EliminarCategoria(int id)
        {
            if (id <= 0)
                return "ID inválido";

            categoriaDAL.Eliminar(id);

            return "OK";
        }

    }
}
