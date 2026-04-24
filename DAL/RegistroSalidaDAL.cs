using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    public class RegistroSalidaDAL
    {
        IconicFashionDbContext _db;

        public int Guardar(RegistroSalida registroSalida, int id = 0, bool esEdicion = false)
        {
            int resultado = 0;

            _db = new IconicFashionDbContext();

            if (esEdicion)
            {
                registroSalida.IdRegistro = id;

                _db.Entry(registroSalida).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                _db.RegistrosSalida.Add(registroSalida);
                _db.SaveChanges();
            }

            resultado = registroSalida.IdRegistro;
            return resultado;
        }

        public List<RegistroSalida> ObtenerRegistrosSalida()
        {
            _db = new IconicFashionDbContext();
            return _db.RegistrosSalida.ToList();
        }

        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            RegistroSalida registroSalida = _db.RegistrosSalida.Find(id);

            if (registroSalida == null)
                return resultado;

            _db.RegistrosSalida.Remove(registroSalida);
            _db.SaveChanges();

            resultado = registroSalida.IdRegistro;

            return resultado;
        }
    }
}
