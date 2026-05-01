using EL;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GUI.Formularios
{
    public partial class FrmUsuarios : Form
    {
        // Instancia global de la lógica de negocio
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
            txtUsuario.Clear();
            txtClaveAcceso.Clear();
            txtTelefono.Text = "";
            Direccion.Clear();
            chkEstado.Checked = true;
            txtNombre.Focus();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            Usuario nuevoUsuario = new Usuario
            {
                Nombre = txtNombre.Text,
                Correo = txtCorreo.Text,
                Username = txtUsuario.Text,
                ClaveAcceso = txtClaveAcceso.Text,
                Telefono = txtTelefono.Text,
                Direccion = Direccion.Text,
                Estado = chkEstado.Checked
            };

            string resultado = usuarioBLL.InsertarUsuario(nuevoUsuario);

            if (resultado == "OK")
            {
                MessageBox.Show("Usuario guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(resultado, "Error al agregar el usuario:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Debe buscar un usuario primero para poder actualizar.");
                return;
            }

            Usuario usuarioEditado = new Usuario
            {
                IdUsuario = Convert.ToInt32(txtId.Text),
                Nombre = txtNombre.Text,
                Correo = txtCorreo.Text,
                Username = txtUsuario.Text,
                ClaveAcceso = txtClaveAcceso.Text,
                Telefono = txtTelefono.Text,
                Direccion = Direccion.Text,
                Estado = chkEstado.Checked
            };

            string resultado = usuarioBLL.ActualizarUsuario(usuarioEditado);

            if (resultado == "OK")
            {
                MessageBox.Show("¡Datos actualizados con éxito!");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                int id = Convert.ToInt32(txtId.Text);
                string resultado = usuarioBLL.EliminarUsuario(id);

                if (resultado == "OK")
                {
                    MessageBox.Show("Usuario eliminado.");
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(resultado);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string idABuscar = Interaction.InputBox("Ingrese el ID del usuario que desea buscar:", "Buscar Usuario", "");

            if (!string.IsNullOrEmpty(idABuscar))
            {
                try
                {
                    int id = Convert.ToInt32(idABuscar);

                    var user = usuarioBLL.ObtenerUsuarioPorId(id);

                    if (user != null)
                    {
                        txtId.Text = user.IdUsuario.ToString();
                        txtNombre.Text = user.Nombre;
                        txtCorreo.Text = user.Correo;
                        txtUsuario.Text = user.Username;
                        txtClaveAcceso.Text = user.ClaveAcceso;
                        txtTelefono.Text = user.Telefono;
                        Direccion.Text = user.Direccion;
                        chkEstado.Checked = user.Estado;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún usuario con el ID: " + id, "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor, ingrese un número de ID válido.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}