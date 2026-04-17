using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBL : IUsuarioBL
    {
        public void AgregarUsuario(Usuario usuario)
        {
            // Validaciones de negocio
            if (usuario == null)
                throw new Exception("El usuario no puede ser nulo");
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new Exception("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new Exception("El correo es obligatorio");

            usuario.FechaRegistro = DateTime.Now;
            usuario.Estado = true;
            UsuarioDAL.AgregarUsuario(usuario);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return UsuarioDAL.ObtenerUsuarios() ?? new List<Usuario>();
        }
        public int ObtenerUltimoId()
        {
            return UsuarioDAL.ObtenerUltimoId();
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            return UsuarioDAL.ObtenerUsuarioPorId(id);
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("El usuario no puede ser nulo");

            var existente = UsuarioDAL.ObtenerUsuarioPorId(usuario.Id);

            if (existente == null)
                throw new Exception("Usuario no encontrado");

            UsuarioDAL.ActualizarUsuario(usuario);
        }

        public void EliminarUsuario(int id)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            var usuario = UsuarioDAL.ObtenerUsuarioPorId(id);

            if (usuario == null)
                throw new Exception("Usuario no existe");

            UsuarioDAL.EliminarUsuario(id);
        }

        public List<Usuario> BuscarUsuarios(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ObtenerUsuarios();

            return UsuarioDAL.BuscarUsuarios(criterio);
        }

        public Usuario Login(string username, string clave)
        {

            return UsuarioDAL.Login(username, clave);
        }
    }
}
