using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {
        // Nota para recordar... esto es una lista que se mantiene viva hasta que se cierra la app completa
        // y no se borra al cerrar el form de clientes.
        public static List<EL.Cliente> clientes;

        public static void AgregarCliente(EL.Cliente cliente)
        {
            if (clientes == null)
            {
                clientes = new List<EL.Cliente>();
            }
            clientes.Add(cliente);
        }
        public static List<EL.Cliente> ObtenerClientes()
        {
            return clientes;
        }
        public static EL.Cliente ObtenerClientePorId(int id)
        {
            return clientes?.FirstOrDefault(c => c.Id == id);
        }
        public static EL.Cliente Login(string username, string clave)
        {
            return clientes?.FirstOrDefault(c => c.Usuario.Username == username && c.Usuario.ClaveAcceso==clave);
        }
        public static void ActualizarCliente(EL.Cliente clienteActualizado)
        {
            var clienteExistente = ObtenerClientePorId(clienteActualizado.Id);
            if (clienteExistente != null)
            {
                clienteExistente.Nombre = clienteActualizado.Nombre;
                clienteExistente.Correo = clienteActualizado.Correo;
                clienteExistente.Telefono = clienteActualizado.Telefono;
                clienteExistente.Direccion = clienteActualizado.Direccion;
                clienteExistente.Estado = clienteActualizado.Estado;
                clienteExistente.FechaModificacion = DateTime.Now;
                clienteExistente.UsuarioModificacion = clienteActualizado.UsuarioModificacion;
            }
        }
        public static void EliminarCliente(int id)
        {
            var cliente = ObtenerClientePorId(id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
            }
        }
        public static List<EL.Cliente> BuscarClientes(string criterio)
        {
            String criterioLower = criterio.ToLower();
            return clientes?.Where(c => c.Nombre.ToLower().Contains(criterioLower) ||
                                        c.Correo.Contains(criterioLower) ||
                                        c.Telefono.Contains(criterioLower)).ToList();
        }
        public static int ObtenerUltimoId()
        {
            if (clientes == null || clientes.Count == 0)
                return 0;

            return clientes.Max(c => c.Id);
        }
    }
}

