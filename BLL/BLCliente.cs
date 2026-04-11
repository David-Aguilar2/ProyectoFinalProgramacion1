using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBL : IClienteBL
    {
        public void AgregarCliente(Cliente cliente)
        {
            // Validaciones de negocio
            if (cliente == null)
                throw new Exception("El cliente no puede ser nulo");

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Correo))
                throw new Exception("El correo es obligatorio");

            cliente.FechaRegistro = DateTime.Now;
            cliente.Estado = true;

            ClienteDAL.AgregarCliente(cliente);
        }

        public List<Cliente> ObtenerClientes()
        {
            return ClienteDAL.ObtenerClientes() ?? new List<Cliente>();
        }
        public int ObtenerUltimoId()
        {
            return ClienteDAL.ObtenerUltimoId();
        }

        public Cliente ObtenerClientePorId(int id)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            return ClienteDAL.ObtenerClientePorId(id);
        }

        public void ActualizarCliente(Cliente cliente)
        {
            if (cliente == null)
                throw new Exception("El cliente no puede ser nulo");

            var existente = ClienteDAL.ObtenerClientePorId(cliente.Id);

            if (existente == null)
                throw new Exception("Cliente no encontrado");

            ClienteDAL.ActualizarCliente(cliente);
        }

        public void EliminarCliente(int id)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            var cliente = ClienteDAL.ObtenerClientePorId(id);

            if (cliente == null)
                throw new Exception("Cliente no existe");

            ClienteDAL.EliminarCliente(id);
        }

        public List<Cliente> BuscarClientes(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ObtenerClientes();

            return ClienteDAL.BuscarClientes(criterio);
        }

        public Cliente Login(string username, string clave)
        {

            return ClienteDAL.Login(username, clave);
        }
    }
}
