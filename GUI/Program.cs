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

            while (true)
            {
                using (var login = new Login())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                BuscarProductos menu = new BuscarProductos();
                Application.Run(menu);

                if (menu.DialogResult != DialogResult.OK)
                {
                    break;
                }

            }

        }
    }
}
