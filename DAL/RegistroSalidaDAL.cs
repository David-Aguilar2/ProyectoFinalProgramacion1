using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class RegistroSalidaDAL
    {
        private IconicFashionDbContext _db;

        public int Guardar(RegistroSalida registroSalida)
        {
            using (_db = new IconicFashionDbContext())
            {
                if (registroSalida.IdRegistro == 0)
                {
                    _db.RegistrosSalida.Add(registroSalida);
                }
                else
                {
                    _db.Entry(registroSalida).State = EntityState.Modified;
                }

                _db.SaveChanges();
                return registroSalida.IdRegistro;
            }
        }

        public List<RegistroSalida> ObtenerRegistrosSalida()
        {
            using (_db = new IconicFashionDbContext())
            {
                return _db.RegistrosSalida
                          .Include(r => r.Producto)
                          .Include(r => r.Usuario)
                          .ToList();
            }
        }

        public void Eliminar(int id)
        {
            using (_db = new IconicFashionDbContext())
            {
                var registro = _db.RegistrosSalida.Find(id);
                if (registro != null)
                {
                    _db.RegistrosSalida.Remove(registro);
                    _db.SaveChanges();
                }
            }
        }
    }
}