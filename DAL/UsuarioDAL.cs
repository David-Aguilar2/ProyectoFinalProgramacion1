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

        public Usuario BuscarPorId(int id)
        {
            _db = new IconicFashionDbContext();
            return _db.Usuarios.Find(id);
        }
        public int Guardar(Usuario usuario)
        {
            _db = new IconicFashionDbContext();

            if (usuario.IdUsuario > 0)
            {
                _db.Entry(usuario).State = EntityState.Modified;
            }
            else
            {
                _db.Usuarios.Add(usuario);
            }

            _db.SaveChanges();
            return usuario.IdUsuario;
        }

        public List<Usuario> ObtenerUsuarios()
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
