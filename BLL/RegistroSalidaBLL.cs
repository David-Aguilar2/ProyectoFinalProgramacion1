using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BLL
{
    public class RegistroSalidaBLL
    {
        public RegistroSalidaDAL registroSalidaDAL = new RegistroSalidaDAL();
        public ProductoBLL productoBLL = new ProductoBLL();

        public string InsertarMovimiento(RegistroSalida registro)
{
    if (registro == null) return "Error: registro vacío";
    
    using (var db = new IconicFashionDbContext())
    {
        var productoBD = db.Productos.Find(registro.IdProducto);
        
        if (productoBD == null) return "El producto no existe en la base de datos.";

        if (registro.Tipo == "SALIDA")
        {
            if (productoBD.Cantidad < registro.Cantidad)
                return $"Stock insuficiente. Solo hay {productoBD.Cantidad} unidades.";
            
            productoBD.Cantidad -= registro.Cantidad;
        }
        else
        {
            productoBD.Cantidad += registro.Cantidad;
        }

        try 
        {
            db.RegistrosSalida.Add(registro);
            
            db.SaveChanges();
            
            return "OK";
        }
        catch (Exception ex)
        {
            return "Error crítico: " + ex.Message;
        }
    }
}

        public List<RegistroSalida> ObtenerRegistrosSalida()
        {
            return registroSalidaDAL.ObtenerRegistrosSalida();
        }

        public string EliminarRegistro(int id)
        {
            if (id <= 0) return "ID inválido";
            registroSalidaDAL.Eliminar(id);
            return "OK";
        }
    }
}