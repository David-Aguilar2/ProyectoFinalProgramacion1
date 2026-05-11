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
    public partial class FrmUsuarios : Form
    {
        // Instancia global de la lógica de negocio
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        private Usuario _usuarioEdicion;

        public FrmUsuarios(Usuario usuario = null)
        {
            InitializeComponent();
            _usuarioEdicion = usuario;
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarComboRoles();

            if (_usuarioEdicion != null)
            {
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

                if (_usuarioEdicion.IdUsuario == 1)
                {
                    cmbRol.Enabled = false;
                    MessageBox.Show("Nota: El rol de este usuario no puede ser modificado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                lblTitulo.Text = "Nuevo Usuario";
                btnAceptar.Text = "Guardar";
                cmbRol.SelectedValue = Usuario.ROL_TRABAJADOR;
            }
        }

        private void CargarComboRoles()
        {
            var userLogueado = Login.UsuarioAutenticado;

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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

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
                u.IdUsuario = _usuarioEdicion.IdUsuario;

                if (u.IdUsuario == 1) u.Rol = Usuario.ROL_SUPERADMIN;

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
                u.ClaveAcceso = UsuarioBLL.EncriptarClave(txtClaveAcceso.Text);
                resultado = usuarioBLL.InsertarUsuario(u);
            }

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