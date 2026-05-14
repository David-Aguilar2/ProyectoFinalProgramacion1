using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL
{
    // Esta clase se encarga de las operaciones reales en la tabla de Registros de Salida (movimientos de inventario)
    public class RegistroSalidaDAL
    {
        private IconicFashionDbContext _db;

        // Guarda un nuevo movimiento o actualiza uno existente en la base de datos
        public int Guardar(RegistroSalida registroSalida)
        {
            // El bloque "using" asegura que la conexión se cierre automáticamente al terminar
            using (_db = new IconicFashionDbContext())
            {
                // Si el ID es 0, es un registro nuevo y lo agregamos a la tabla
                if (registroSalida.IdRegistro == 0)
                {
                    _db.RegistrosSalida.Add(registroSalida);
                }
                // Si el ID ya existe, indicamos que solo vamos a modificar los datos
                else
                {
                    _db.Entry(registroSalida).State = EntityState.Modified;
                }

                // Aplicamos los cambios en la base de datos
                _db.SaveChanges();
                return registroSalida.IdRegistro;
            }
        }

        // Trae la lista de todos los movimientos (entradas y salidas) registrados
        public List<RegistroSalida> ObtenerRegistrosSalida()
        {
            using (_db = new IconicFashionDbContext())
            {
                // ".Include" se usa para traer también el nombre del Producto y del Usuario 
                // que realizó la operación, evitando que esos datos salgan vacíos.
                return _db.RegistrosSalida
                          .Include(r => r.Producto)
                          .Include(r => r.Usuario)
                          .ToList();
            }
        }

        // Borra un registro del historial usando su número de ID
        public void Eliminar(int id)
        {
            using (_db = new IconicFashionDbContext())
            {
                // Primero buscamos si el registro existe
                var registro = _db.RegistrosSalida.Find(id);

                // Si lo encuentra, lo elimina de la tabla y guarda el cambio
                if (registro != null)
                {
                    _db.RegistrosSalida.Remove(registro);
                    _db.SaveChanges();
                }
            }
        }
    }
}