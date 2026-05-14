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
    // Formulario encargado de validar la identidad de los usuarios al entrar
    public partial class Login : Form
    {
        // Conexión con la lógica de negocios de usuarios
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        // Propiedad global para recordar qué usuario entró al sistema en esta sesión
        public static Usuario UsuarioAutenticado { get; set; }

        public Login()
        {
            InitializeComponent();
        }

        // Evento que se dispara al hacer clic en el botón de entrar
        private void IniciarSesion_Click(object sender, EventArgs e)
        {
            // Captura los datos escritos eliminando espacios vacíos al inicio o final
            string user = usuario.Text.Trim();
            string pass = password.Text.Trim();

            // Valida que el usuario no haya dejado los campos en blanco
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor, ingrese sus credenciales.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convierte la contraseña escrita a su código secreto para compararla con la base de datos
            string passHasheada = UsuarioBLL.EncriptarClave(pass);

            // Intenta buscar un usuario que coincida con ese nombre y clave
            var usuarioLogueado = usuarioBLL.Login(user, passHasheada);

            // Si el usuario existe en la base de datos...
            if (usuarioLogueado != null)
            {
                // Verifica si el usuario tiene permiso de entrar (Estado Activo)
                if (!usuarioLogueado.Estado)
                {
                    MessageBox.Show("Este usuario se encuentra desactivado.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Guarda los datos del usuario en la variable global para usarlo en otros formularios
                UsuarioAutenticado = usuarioLogueado;

                // Prepara un mensaje de bienvenida personalizado según el rango
                string mensajeBienvenida = $"Bienvenido {usuarioLogueado.Nombre}\r\nRol: ";

                switch (usuarioLogueado.Rol)
                {
                    case Usuario.ROL_SUPERADMIN: mensajeBienvenida += "Administrador Absoluto"; break;
                    case Usuario.ROL_ADMIN: mensajeBienvenida += "Administrador"; break;
                    case Usuario.ROL_TRABAJADOR: mensajeBienvenida += "Trabajador"; break;
                }

                MessageBox.Show(mensajeBienvenida, "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Avisa al programa que el login fue exitoso y cierra esta ventana
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Si los datos son incorrectos, avisa al usuario y limpia las cajas de texto
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                usuario.Clear();
                password.Clear();
                usuario.Focus(); // Pone el cursor de nuevo en el usuario para reintentar
            }
        }
    }
}