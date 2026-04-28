using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        UsuarioDAL dal = new UsuarioDAL();

        //Insertar
        public string InsertarUsuario(Usuario usuario)
        {
            if (usuario == null)
                return "Error: usuario vacío";

            if (string.IsNullOrWhiteSpace(usuario.Username))
                return "El nombre de usuario es obligatorio";

            if (string.IsNullOrWhiteSpace(usuario.ClaveAcceso))
                return "La contraseña es obligatoria";

            if (usuario.IdUsuario <= 0)
                return "Debe seleccionar un usuario válido";

            // Validar formato de usuario 
            if (usuario.Username.Length < 4)
                return "El usuario debe tener al menos 4 caracteres";

            // Validar duplicados
            var lista = dal.ObtenerUsuarios();
            if (lista.Any(u => u.Username.ToLower() == usuario.Username.ToLower()))
                return "Ya existe un usuario con ese nombre";

            dal.Guardar(usuario);

            return "OK";
        }

        // Actualizar
        public string ActualizarUsuario(Usuario usuario)
        {
            if (usuario == null)
                return "Error: usuario vacío";

            if (usuario.IdUsuario <= 0)
                return "Usuario inválido";

            if (string.IsNullOrWhiteSpace(usuario.Username))
                return "El nombre de usuario es obligatorio";

            if (string.IsNullOrWhiteSpace(usuario.ClaveAcceso))
                return "La contraseña es obligatoria";

            if (usuario.IdUsuario <= 0)
                return "Usuario inválido";

            dal.Guardar(usuario, usuario.IdUsuario, true);

            return "OK";
        }

        // Eliminar
        public string EliminarUsuario(int id)
        {
            if (id <= 0)
                return "ID inválido";

            dal.Eliminar(id);

            return "OK";
        }

        //Consultar
        public List<Usuario> ObtenerUsuarios()
        {
            return dal.ObtenerUsuarios();
        }
    }
}
