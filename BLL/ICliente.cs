using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IClienteBL
    {
        void AgregarCliente(Cliente cliente);
        List<Cliente> ObtenerClientes();
        Cliente ObtenerClientePorId(int id);
        Cliente Login(String username,String clave);
        int ObtenerUltimoId();
        void ActualizarCliente(Cliente cliente);
        void EliminarCliente(int id);
        List<Cliente> BuscarClientes(string criterio);
    }
}
