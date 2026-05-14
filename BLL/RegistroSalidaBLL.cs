using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BLL
{
    // Esta clase contiene las reglas y validaciones antes de guardar o borrar movimientos de inventario (entradas y salidas)
    public class RegistroSalidaBLL
    {
        // Conexiones con las capas de datos y de productos
        public RegistroSalidaDAL registroSalidaDAL = new RegistroSalidaDAL();
        public ProductoBLL productoBLL = new ProductoBLL();

        // Registra un movimiento y actualiza automáticamente el stock del producto
        public string InsertarMovimiento(RegistroSalida registro)
        {
            // Valida que el registro no venga vacío
            if (registro == null) return "Error: registro vacío";

            // Abre la conexión a la base de datos
            using (var db = new IconicFashionDbContext())
            {
                // Busca el producto en la base de datos para modificar su cantidad
                var productoBD = db.Productos.Find(registro.IdProducto);

                // Si el producto no existe, detiene el proceso
                if (productoBD == null) return "El producto no existe en la base de datos.";

                // Si el movimiento es una SALIDA de mercancía...
                if (registro.Tipo == "SALIDA")
                {
                    // Verifica que tengamos suficientes existencias para vender o retirar
                    if (productoBD.Cantidad < registro.Cantidad)
                        return $"Stock insuficiente. Solo hay {productoBD.Cantidad} unidades.";

                    // Resta la cantidad al stock actual
                    productoBD.Cantidad -= registro.Cantidad;
                }
                // Si el movimiento es una ENTRADA de mercancía...
                else
                {
                    // Suma la cantidad al stock actual
                    productoBD.Cantidad += registro.Cantidad;
                }

                try
                {
                    // Agrega el historial del movimiento a la tabla de registros
                    db.RegistrosSalida.Add(registro);

                    // Guarda todos los cambios (el nuevo stock y el registro del movimiento)
                    db.SaveChanges();

                    return "OK";
                }
                catch (Exception ex)
                {
                    // En caso de un fallo técnico, devuelve el mensaje de error
                    return "Error crítico: " + ex.Message;
                }
            }
        }

        // Trae la lista completa de todos los movimientos de inventario realizados
        public List<RegistroSalida> ObtenerRegistrosSalida()
        {
            return registroSalidaDAL.ObtenerRegistrosSalida();
        }

        // Elimina un registro del historial usando su número de ID
        public string EliminarRegistro(int id)
        {
            // Valida que el ID sea un número correcto
            if (id <= 0) return "ID inválido";

            registroSalidaDAL.Eliminar(id);
            return "OK";
        }
    }
}