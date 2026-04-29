using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RegistroSalidaBLL
    {
        public RegistroSalidaBLL() { }
        public RegistroSalidaDAL registroSalidaDAL = new RegistroSalidaDAL();
        public string InsertarRegistroSalida(RegistroSalida registro)
        {
            if (registro == null)
                return "Error: registro vacío";

            if (registro.Producto == null || registro.IdProducto <= 0)
                return "Debe seleccionar un producto válido";

            if (registro.Usuario == null || registro.IdUsuario <= 0)
                return "Debe seleccionar un usuario válido";

            if (string.IsNullOrWhiteSpace(registro.NombreCliente))
                return "El nombre del cliente es obligatorio";

            if (string.IsNullOrWhiteSpace(registro.TelefonoCliente))
                return "El teléfono del cliente es obligatorio";

            if (registro.Cantidad <= 0)
                return "La cantidad debe ser mayor a 0";

            // Asignar fecha actual si no viene definida
            if (registro.FechaSalida == default(DateTime))
                registro.FechaSalida = DateTime.Now;

            registroSalidaDAL.Guardar(registro);

            return "OK";
        }

        public string ActualizarRegistroSalida(RegistroSalida registro)
        {
            if (registro == null)
                return "Error: registro vacío";

            if (registro.IdRegistro <= 0)
                return "Registro inválido";

            if (registro.Producto == null || registro.IdProducto <= 0)
                return "Debe seleccionar un producto válido";

            if (registro.Usuario == null || registro.IdUsuario <= 0)
                return "Debe seleccionar un usuario válido";

            if (string.IsNullOrWhiteSpace(registro.NombreCliente))
                return "El nombre del cliente es obligatorio";

            if (string.IsNullOrWhiteSpace(registro.TelefonoCliente))
                return "El teléfono del cliente es obligatorio";

            if (registro.Cantidad <= 0)
                return "La cantidad debe ser mayor a 0";

            if (registro.FechaSalida == default(DateTime))
                return "La fecha de salida es obligatoria";


            registroSalidaDAL.Guardar(registro);

            return "OK";
        }

        public List<EL.RegistroSalida> ObtenerRegistrosSalida()
        {
            return registroSalidaDAL.ObtenerRegistrosSalida();
        }
        public string EliminarCategoria(int id)
        {
            if (id <= 0)
                return "ID inválido";

            registroSalidaDAL.Eliminar(id);

            return "OK";
        }


    }
}
