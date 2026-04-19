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

        private void IniciarSesion_Click(object sender, EventArgs e)
        {
            if (usuario.Text == "admin" && password.Text == "admin")
            {

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Mostrar un mensaje de error si fallan las credenciales
                MessageBox.Show("Error, usuario o contraseña incorrectos.", "Iconic Fashion | Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Limpiar los campos y regresar el foco al usuario
                usuario.Clear();
                password.Clear();
                usuario.Focus();
            }
        }
    }
}
