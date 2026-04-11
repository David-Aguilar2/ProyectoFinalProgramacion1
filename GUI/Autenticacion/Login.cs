using BLL;
using EL;
using GUI.Menu_Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Autenticacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }
        ClienteBL bl = new ClienteBL();
        public static String usuarioActual;

        BuscarProductos frmMenu;

        private void IniciarSesion_Click(object sender, EventArgs e)
        {
            Cliente clienteLogueado = bl.Login(usuario.Text, password.Text);
            //Validar las credenciales
            if (clienteLogueado!=null)
            {
                //Mostrar el menú principal

                //Vamos a agregar el valor DialogResult.OK a la propiedad DialogResult del formulario Login
                //this.DialogResult = DialogResult.OK;
                //Cerrar el formulario
                this.Hide();
                frmMenu = new BuscarProductos();
                frmMenu.ShowDialog();
            }
            else
            {
                //Mostrar un mensaje de error
                MessageBox.Show("Error, usuario o contraseña incorrectos.", "Iconic Fashion | Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Limpiar los txt y enfocar el txtUsuario
                usuario.Clear();
                password.Clear();

                usuario.Focus();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

            Cliente c = new Cliente
            {
                Id = 1,
                Nombre = "admin",
                Correo = "admin",
                Telefono = "7777-7777",
                Direccion = "N/A",
                Estado = true,
                UsuarioRegistro = null,
                FechaRegistro = DateTime.Now      ,
                Usuario = new Usuario
                {
                    Id = bl.ObtenerUltimoId()+1,
                    Username = "admin",
                    ClaveAcceso = "admin"                   
                }
            };

            bl.AgregarCliente(c);

        }
    }
}
