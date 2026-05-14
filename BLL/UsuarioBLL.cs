using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        UsuarioDAL dal = new UsuarioDAL();

        //Buscar por ID
        public Usuario ObtenerUsuarioPorId(int id)
        {
            if (id <= 0) return null;
            return dal.BuscarPorId(id);
        }

        public Usuario Login(string user, string pass)
        {
            return dal.ObtenerUsuarios().FirstOrDefault(u => u.Username == user && u.ClaveAcceso == pass);
        }

        public static string EncriptarClave(string clave)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clave));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //Insertar
        public string InsertarUsuario(Usuario usuario)
        {
            if (usuario.IdUsuario == 0 && usuario.Rol == Usuario.ROL_SUPERADMIN)
            {
                return "No se permite la creación de Administradores Absolutos por seguridad.";
            }

            if (usuario.IdUsuario == 1 && usuario.Rol != Usuario.ROL_SUPERADMIN)
            {
                return "El Administrador Absoluto no puede ser degradado de rango.";
            }

            if (usuario == null)
                return "Error: usuario vacío";

            if (string.IsNullOrWhiteSpace(usuario.Username))
                return "El nombre de usuario es obligatorio";

            if (string.IsNullOrWhiteSpace(usuario.ClaveAcceso))
                return "La contraseña es obligatoria";

            if (usuario.ClaveAcceso.Length < 5)
                return "La contraseña debe tener al menos 5 caracteres";

            // Validar formato de usuario 
            if (usuario.Username.Length < 4)
                return "El usuario debe tener al menos 4 caracteres";

            if (!string.IsNullOrWhiteSpace(usuario.Correo))
            {
                // Explicación: Verifica que tenga formato texto@texto.dominio
                string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(usuario.Correo, patron))
                {
                    return "El formato del correo electrónico no es válido";
                }
            }
            else
            {
                return "El correo electrónico es obligatorio";
            }

            if (string.IsNullOrWhiteSpace(usuario.Telefono) || usuario.Telefono.Length < 9)
            {
                return "El número de teléfono debe estar completo (Formato: 0000-0000)";
            }

            // Validar duplicados
            var lista = dal.ObtenerUsuarios();
            if (lista.Any(u => u.Username.ToLower() == usuario.Username.ToLower()))
                return "Ya existe un usuario con ese nombre";

            if(lista.Any(u => u.Correo.ToLower() == usuario.Correo.ToLower()))
                return "Ya existe un registro con este correo electrónico.";

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

            // Validar formato de usuario 
            if (usuario.Username.Length < 4)
                return "El usuario debe tener al menos 4 caracteres";

            if (string.IsNullOrWhiteSpace(usuario.Telefono) || usuario.Telefono.Length < 9)
            {
                return "El número de teléfono debe estar completo (Formato: 0000-0000)";
            }

            var lista = dal.ObtenerUsuarios();

            if (lista.Any(u => u.Username.ToLower() == usuario.Username.ToLower() && u.IdUsuario != usuario.IdUsuario))
                return "Ya existe ese usuario.";

            if (lista.Any(u => u.Correo.ToLower() == usuario.Correo.ToLower() && u.IdUsuario != usuario.IdUsuario))
            {
                return "No se puede actualizar: El correo electrónico ya pertenece a otro usuario.";
            }

            dal.Guardar(usuario);

            return "OK";
        }

        // Eliminar
        public string EliminarUsuario(int id)
        {
            if (id == 1)
            {
                return "El Administrador Absoluto no puede ser eliminado del sistema.";
            }

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
