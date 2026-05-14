using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Esta clase contiene las reglas y validaciones antes de guardar o borrar categorías
    public class CategoriaBLL
    {
        public CategoriaBLL() { }

        // Conexión con la capa de datos de categorías
        public CategoriaDAL categoriaDAL = new CategoriaDAL();

        // Busca una categoría específica usando su número de ID
        public Categoria ObtenerCategoriaPorId(int id)
        {
            return categoriaDAL.ObtenerCategoria(id);
        }

        // Registra una nueva categoría validando que los datos estén completos y no repetidos
        public string InsertarCategoria(Categoria categoria)
        {
            // Revisa que el objeto categoría no llegue vacío
            if (categoria == null)
                return "Error: categoría vacía";

            // Verifica que se haya escrito un nombre
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                return "El nombre de la categoría es obligatorio";

            // Verifica que se haya escrito una descripción
            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                return "La descripción de la categoría es obligatoria";

            // Revisa en la base de datos que no exista otra categoría con el mismo nombre
            var lista = categoriaDAL.ObtenerCategorias();
            if (lista.Any(c => c.Nombre.ToLower() == categoria.Nombre.ToLower()))
                return "Ya existe una categoría con ese nombre";

            // Si todo está correcto, manda la información a la capa de datos para guardarla
            categoriaDAL.Guardar(categoria);

            return "OK";
        }

        // Modifica una categoría existente validando que los nuevos datos sean correctos
        public string ActualizarCategoria(Categoria categoria)
        {
            if (categoria == null)
                return "Error: categoría vacía";

            // Verifica que el ID sea válido para poder encontrarla
            if (categoria.IdCategoria <= 0)
                return "Categoría inválida";

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                return "El nombre de la categoría es obligatorio";

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                return "La descripción de la categoría es obligatoria";

            // Verifica que el nuevo nombre no lo tenga ya otra categoría distinta
            var lista = categoriaDAL.ObtenerCategorias();
            if (lista.Any(c => c.Nombre.ToLower() == categoria.Nombre.ToLower()
                               && c.IdCategoria != categoria.IdCategoria))
                return "Ya existe otra categoría con ese nombre";

            // Manda a actualizar indicando el ID y que se trata de una edición
            categoriaDAL.Guardar(categoria, categoria.IdCategoria, true);

            return "OK";
        }

        // Trae la lista completa de todas las categorías disponibles
        public List<Categoria> ObtenerCategorias()
        {
            return categoriaDAL.ObtenerCategorias();
        }

        // Borra una categoría del sistema usando su ID
        public string EliminarCategoria(int id)
        {
            if (id <= 0)
                return "ID inválido";

            categoriaDAL.Eliminar(id);

            return "OK";
        }
    }
}