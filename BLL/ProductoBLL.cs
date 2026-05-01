using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoBLL 
    {
        public ProductoBLL() { }
        ProductoDAL dal = new ProductoDAL();

        //Buscar por ID
        public Producto ObtenerProductoPorId(int id)
        {
            if (id <= 0)
                return null;
            return dal.ObtenerProducto(id);
        }

        //Insetar  validaciones
        public string InsertarProducto(Producto producto)
        {
            // Validar que el producto no sea nulo
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return "El nombre del producto es obligatorio";

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                return "La descripción es obligatoria";

            if (producto.Precio <= 0)
                return "El precio debe ser mayor a 0";

            if (producto.Cantidad < 0)
                return "La cantidad no puede ser negativa";

            if (producto.IdCategoria <= 0)
                return "Debe seleccionar una categoría válida";

            // Validar duplicados
            var lista = dal.ObtenerProductos();
            if (lista.Any(p => p.Nombre.ToLower() == producto.Nombre.ToLower()))
                return "Ya existe un producto con ese nombre";

            producto.FechaRegistro = DateTime.Now;

            dal.Guardar(producto);

            return "OK";
        }

        //Actualizar
        public string ActualizarProducto(Producto producto)
        {
            if (producto == null)
                return "Error: producto vacío";

            if (producto.IdProducto <= 0)
                return "Producto inválido";

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return "El nombre es obligatorio";

            if (producto.Precio <= 0)
                return "El precio debe ser mayor a 0";

            if (producto.Cantidad < 0)
                return "Cantidad inválida";

            if (producto.IdCategoria <= 0)
                return "Debe seleccionar una categoría válida";

            dal.Guardar(producto);

            return "OK";
        }

        // Eliminar
        public string EliminarProducto(int id)
        {
            if (id <= 0)
                return "ID inválido";

            dal.Eliminar(id);

            return "OK";
        }
    }
}
