using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IUsuarioBL
    {
        void AgregarUsuario(Usuario usuario);
        List<Usuario> ObtenerUsuarios();
       Usuario ObtenerUsuarioPorId (int id);
        Usuario Login(String username,String clave);
        int ObtenerUltimoId();
        void ActualizarUsuario(Usuario usuario);
        void EliminarUsuario(int id);
        List<Usuario> BuscarUsuarios(string criterio);
    }
}
