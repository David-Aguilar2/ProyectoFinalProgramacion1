using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    public class UsuarioDAL
    {
        IconicFashionDbContext _db;

        public int Guardar(Usuario usuario, int id = 0, bool esEdicion = false)
        {
            int resultado = 0;

            _db = new IconicFashionDbContext();

            if (esEdicion)
            {
                usuario.IdUsuario = id;

                _db.Entry(usuario).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                _db.Usuarios.Add(usuario);
                _db.SaveChanges();
            }

            resultado = usuario.IdUsuario;
            return resultado;
        }

        public List<Usuario> ObtenerProductos()
        {
            _db = new IconicFashionDbContext();
            return _db.Usuarios.ToList();
        }

        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            Usuario usuario = _db.Usuarios.Find(id);

            if (usuario == null)
                return resultado;

            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();

            resultado = usuario.IdUsuario;

            return resultado;
        }
    }
}
