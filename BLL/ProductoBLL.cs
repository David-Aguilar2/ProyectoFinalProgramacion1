using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Esta clase contiene las reglas y validaciones antes de guardar o borrar productos
    public class ProductoBLL
    {
        // Conexión con la capa de datos de productos
        public ProductoBLL() { }
        ProductoDAL dal = new ProductoDAL();

        // Busca un producto por su número de ID, validando que el número sea correcto
        public Producto ObtenerProductoPorId(int id)
        {
            if (id <= 0)
                return null;
            return dal.ObtenerProducto(id);
        }

        // Registra un producto nuevo asegurando que los datos básicos sean correctos
        public string InsertarProducto(Producto producto)
        {
            // Revisa que el nombre y la descripción no estén vacíos
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return "El nombre del producto es obligatorio";

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                return "La descripción es obligatoria";

            // El precio debe tener un valor real (mayor a cero)
            if (producto.Precio <= 0)
                return "El precio debe ser mayor a 0";

            // No se puede iniciar un producto con existencias negativas
            if (producto.Cantidad < 0)
                return "La cantidad no puede ser negativa";

            // Obliga a que el producto pertenezca a una categoría de la lista
            if (producto.IdCategoria <= 0)
                return "Debe seleccionar una categoría válida";

            // Revisa que no se repita el nombre con otro producto ya guardado
            var lista = dal.ObtenerProductos();
            if (lista.Any(p => p.Nombre.ToLower() == producto.Nombre.ToLower()))
                return "Ya existe un producto con ese nombre";

            // Asigna la fecha y hora actual como momento de registro
            producto.FechaRegistro = DateTime.Now;

            dal.Guardar(producto);
            return "OK";
        }

        // Actualiza un producto y crea automáticamente un registro de movimiento si cambia el stock
        public string ActualizarProducto(Producto productoEditado, int idUsuario)
        {
            using (var db = new IconicFashionDbContext())
            {
                try
                {
                    // Busca el producto tal como está guardado actualmente en la base de datos
                    var productoOriginal = db.Productos.Find(productoEditado.IdProducto);
                    if (productoOriginal == null) return "Producto no encontrado";

                    // LOGICA DE INVENTARIO: Si la cantidad nueva es distinta a la que había...
                    if (productoOriginal.Cantidad != productoEditado.Cantidad)
                    {
                        // Calcula cuántas unidades de diferencia hay
                        int diferencia = productoEditado.Cantidad - productoOriginal.Cantidad;

                        // Crea un reporte automático de lo que pasó
                        RegistroSalida movimientoAuto = new RegistroSalida
                        {
                            IdProducto = productoEditado.IdProducto,
                            IdUsuario = idUsuario,
                            FechaSalida = DateTime.Now,
                            Motivo = "Stock actualizado por administrador",
                            // Si la diferencia es positiva es ENTRADA, si es negativa es SALIDA
                            Tipo = diferencia > 0 ? "ENTRADA" : "SALIDA",
                            Cantidad = Math.Abs(diferencia) // Guarda el número siempre en positivo
                        };

                        db.RegistrosSalida.Add(movimientoAuto);
                    }

                    // Actualiza todos los datos del producto original con los nuevos cambios
                    db.Entry(productoOriginal).CurrentValues.SetValues(productoEditado);

                    db.SaveChanges();
                    return "OK";
                }
                catch (Exception ex)
                {
                    return "Error al actualizar: " + ex.Message;
                }
            }
        }

        // Borra un producto del sistema usando su ID
        public string EliminarProducto(int id)
        {
            if (id <= 0)
                return "ID inválido";

            dal.Eliminar(id);
            return "OK";
        }

        // Trae la lista de todos los productos disponibles
        public List<Producto> ObtenerProductos()
        {
            return dal.ObtenerProductos();
        }
    }
}