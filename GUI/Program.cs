using GUI.Autenticacion;
using GUI.Menu_Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool loginExitoso = false;
            using (Login formLogin = new Login())
            {
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    loginExitoso = true;
                }
            }

            if (loginExitoso)
            {
                // Forzamos la ejecución del Menú
                Application.Run(new BuscarProductos());
            }

        }
    }
}
