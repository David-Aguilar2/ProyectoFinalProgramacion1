using BLL;
using DAL;
using EL;
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
                btnAceptar.Text = "Actualizar";
            }
            else
            {
                lblTitulo.Text = "Nuevo Usuario";
                btnAceptar.Text = "Guardar";
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
                ClaveAcceso = txtClaveAcceso.Text,
                Telefono = txtTelefono.Text,
                Direccion = Direccion.Text,
                Estado = chkEstado.Checked
            };

            string resultado;

            if (_usuarioEdicion != null)
            {
                u.IdUsuario = _usuarioEdicion.IdUsuario;
                resultado = usuarioBLL.ActualizarUsuario(u);
            }
            else
            {
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