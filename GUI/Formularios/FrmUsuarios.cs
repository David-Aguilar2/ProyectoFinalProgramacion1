using BLL;
using DAL;
using EL;
using GUI.Autenticacion;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Formularios
{
    // Formulario para crear y editar los perfiles de los usuarios que usan el sistema
    public partial class FrmUsuarios : Form
    {
        // Conexión con la lógica de negocio de usuarios
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        // Variable para guardar temporalmente al usuario que se va a editar
        private Usuario _usuarioEdicion;

        public FrmUsuarios(Usuario usuario = null)
        {
            InitializeComponent();
            _usuarioEdicion = usuario;
        }

        // Configura el formulario al abrirse (Nuevo o Editar)
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarComboRoles(); // Llena la lista de roles permitidos

            if (_usuarioEdicion != null)
            {
                // Si es edición, carga todos los datos del usuario en los campos
                lblTitulo.Text = "Editar Usuario";
                txtId.Text = _usuarioEdicion.IdUsuario.ToString();
                txtNombre.Text = _usuarioEdicion.Nombre;
                txtCorreo.Text = _usuarioEdicion.Correo;
                txtUsuario.Text = _usuarioEdicion.Username;
                txtClaveAcceso.Text = _usuarioEdicion.ClaveAcceso;
                txtTelefono.Text = _usuarioEdicion.Telefono;
                Direccion.Text = _usuarioEdicion.Direccion;
                chkEstado.Checked = _usuarioEdicion.Estado;
                cmbRol.SelectedValue = _usuarioEdicion.Rol;
                btnAceptar.Text = "Actualizar";

                // Protección especial: El dueño del sistema (ID 1) no puede cambiar su propio rango
                if (_usuarioEdicion.IdUsuario == 1)
                {
                    cmbRol.Enabled = false;
                    MessageBox.Show("Nota: El rol de este usuario no puede ser modificado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Si es un usuario nuevo, pone valores por defecto
                lblTitulo.Text = "Nuevo Usuario";
                btnAceptar.Text = "Guardar";
                cmbRol.SelectedValue = Usuario.ROL_TRABAJADOR;
            }
        }

        // Regla de jerarquía: Un usuario solo puede crear otros usuarios de menor rango que él
        private void CargarComboRoles()
        {
            var userLogueado = Login.UsuarioAutenticado;

            // El Administrador Absoluto puede crear Administradores y Trabajadores
            if (userLogueado.Rol == Usuario.ROL_SUPERADMIN)
            {
                var roles = new[]
                {
                    new { Id = Usuario.ROL_ADMIN, Nombre = "Administrador" },
                    new { Id = Usuario.ROL_TRABAJADOR, Nombre = "Trabajador" }
                }.ToList();

                cmbRol.DataSource = roles;
                cmbRol.ValueMember = "Id";
                cmbRol.DisplayMember = "Nombre";
            }
            // Un Administrador normal solo puede crear Trabajadores
            else if (userLogueado.Rol == Usuario.ROL_ADMIN)
            {
                var roles = new[]
                 {
                    new { Id = Usuario.ROL_TRABAJADOR, Nombre = "Trabajador" }
                }.ToList();

                cmbRol.DataSource = roles;
                cmbRol.ValueMember = "Id";
                cmbRol.DisplayMember = "Nombre";
            }
        }

        // Guarda o actualiza la información del usuario
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Validación rápida de nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            // Captura los datos de la pantalla
            Usuario u = new Usuario
            {
                Nombre = txtNombre.Text,
                Correo = txtCorreo.Text,
                Username = txtUsuario.Text,
                Telefono = txtTelefono.Text,
                Direccion = Direccion.Text,
                Estado = chkEstado.Checked,
                Rol = Convert.ToInt32(cmbRol.SelectedValue)
            };

            string resultado;

            if (_usuarioEdicion != null)
            {
                // Si es edición, mantenemos su ID
                u.IdUsuario = _usuarioEdicion.IdUsuario;

                // Forzamos que el ID 1 siempre sea SuperAdmin por seguridad
                if (u.IdUsuario == 1) u.Rol = Usuario.ROL_SUPERADMIN;

                // Si el usuario cambió la contraseña, la encriptamos de nuevo
                // Si dejó la misma, la pasamos tal cual para no re-encriptar algo ya encriptado
                if (txtClaveAcceso.Text != _usuarioEdicion.ClaveAcceso)
                {
                    u.ClaveAcceso = UsuarioBLL.EncriptarClave(txtClaveAcceso.Text);
                }
                else
                {
                    u.ClaveAcceso = _usuarioEdicion.ClaveAcceso;
                }

                resultado = usuarioBLL.ActualizarUsuario(u);
            }
            else
            {
                // Si es nuevo, encriptamos su contraseña por primera vez
                u.ClaveAcceso = UsuarioBLL.EncriptarClave(txtClaveAcceso.Text);
                resultado = usuarioBLL.InsertarUsuario(u);
            }

            // Verifica el resultado y cierra la ventana si todo salió bien
            if (resultado == "OK")
            {
                MessageBox.Show("Operación realizada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}