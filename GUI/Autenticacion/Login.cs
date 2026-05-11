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
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public static Usuario UsuarioAutenticado { get; set; }
        public Login()
        {
            InitializeComponent();

        }

        private void IniciarSesion_Click(object sender, EventArgs e)
        {
            string user = usuario.Text.Trim();
            string pass = password.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor, ingrese sus credenciales.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string passHasheada = UsuarioBLL.EncriptarClave(pass);

            var usuarioLogueado = usuarioBLL.Login(user, passHasheada);

            if (usuarioLogueado != null)
            {
                if (!usuarioLogueado.Estado)
                {
                    MessageBox.Show("Este usuario se encuentra desactivado.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                UsuarioAutenticado = usuarioLogueado;

                string mensajeBienvenida = $"Bienvenido {usuarioLogueado.Nombre}\r\nRol: ";

                switch (usuarioLogueado.Rol)
                {
                    case Usuario.ROL_SUPERADMIN: mensajeBienvenida += "Administrador Absoluto"; break;
                    case Usuario.ROL_ADMIN: mensajeBienvenida += "Administrador"; break;
                    case Usuario.ROL_TRABAJADOR: mensajeBienvenida += "Trabajador"; break;
                }

                MessageBox.Show(mensajeBienvenida, "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                usuario.Clear();
                password.Clear();
                usuario.Focus();
            }
        }
    }
}
