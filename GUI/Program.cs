using GUI.Autenticacion;
using GUI.Menu_Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    // Esta es la clase principal que arranca todo el programa
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread] // Indica que el modelo de hilos de la aplicación es de un solo hilo (necesario para Windows Forms)
        static void Main()
        {
            Application.EnableVisualStyles(); // Mejora la apariencia visual de los controles
            Application.SetCompatibleTextRenderingDefault(false);

            // CICLO DE SESIÓN: Este bucle permite que el programa regrese al Login 
            // si el usuario decide "Cerrar Sesión" en lugar de salir del programa.
            while (true)
            {
                // 1. Mostrar la ventana de Login
                using (var login = new Login())
                {
                    // Si el usuario cierra el login con la 'X' o cancela, el programa termina
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                // 2. Si el Login fue exitoso, abrimos el Menú Principal (Dashboard)
                BuscarProductos menu = new BuscarProductos();
                
                // Application.Run mantiene abierta la ventana principal hasta que se cierre
                Application.Run(menu);

                // 3. Control de salida:
                // Si el menú se cerró con DialogResult.OK (esto lo configuramos en el botón "Cerrar Sesión"),
                // el bucle vuelve al inicio y muestra el Login otra vez.
                // Si se cerró de cualquier otra forma (por la X o salir), el bucle se rompe y la app finaliza.
                if (menu.DialogResult != DialogResult.OK)
                {
                    break;
                }
            }
        }
    }
}