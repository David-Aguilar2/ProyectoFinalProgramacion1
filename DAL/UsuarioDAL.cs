using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    // Esta clase se encarga de las operaciones reales en la tabla de Usuarios
    public class UsuarioDAL
    {
        // Conexión a la base de datos
        IconicFashionDbContext _db;

        // Busca a un usuario específico usando su número de ID
        public Usuario BuscarPorId(int id)
        {
            _db = new IconicFashionDbContext();
            return _db.Usuarios.Find(id);
        }

        // Este método sirve tanto para crear usuarios nuevos como para editar los existentes
        public int Guardar(Usuario usuario)
        {
            _db = new IconicFashionDbContext();

            // Si el usuario ya tiene un ID, significa que ya existe y solo vamos a cambiar sus datos
            if (usuario.IdUsuario > 0)
            {
                _db.Entry(usuario).State = EntityState.Modified;
            }
            // Si el ID es 0, es un usuario nuevo y lo agregamos a la lista
            else
            {
                _db.Usuarios.Add(usuario);
            }

            // Guarda los cambios definitivamente en la base de datos
            _db.SaveChanges();
            return usuario.IdUsuario;
        }

        // Trae la lista completa de todos los usuarios registrados
        public List<Usuario> ObtenerUsuarios()
        {
            _db = new IconicFashionDbContext();
            return _db.Usuarios.ToList();
        }

        // Borra a un usuario del sistema usando su ID
        public int Eliminar(int id)
        {
            int resultado = 0;
            _db = new IconicFashionDbContext();

            // Primero buscamos si el usuario realmente existe
            Usuario usuario = _db.Usuarios.Find(id);

            // Si no lo encuentra, no hace nada
            if (usuario == null)
                return resultado;

            // Si lo encuentra, lo borra y guarda el cambio
            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();

            resultado = usuario.IdUsuario;

            return resultado;
        }
    }
}