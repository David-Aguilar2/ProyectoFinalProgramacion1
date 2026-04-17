using BLL;
using EL;
using GUI.Autenticacion;
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

namespace GUI
{
    public partial class FrmUsuario : Form
    {
       

        ClienteBL bl = new ClienteBL();
        Cliente cliente;
        DataGridViewRow filaSeleccionada;

        public FrmUsuario()
        {
            InitializeComponent(); 
        }

        private void Cliente_Load(object sender, EventArgs e)
        {
            CargarClientes();
            txtNombre.Focus();
        }

        //Eventos
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente c = new Cliente
                {
                    Id = bl.ObtenerUltimoId() + 1,
                    Nombre = txtNombre.Text,
                    Correo = txtCorreo.Text,
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    Estado = chkEstado.Checked,
                    UsuarioRegistro = Login.usuarioActual,
                    FechaRegistro = DateTime.Now,
                    Usuario = new Usuario { Id = 1, Username = txtUsuario.Text, ClaveAcceso = txtClaveAcceso.Text, }
                };

                bl.AgregarCliente(c);

                CargarClientes();
                LimpiarCampos();

                MessageBox.Show("Cliente agregado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente c = new Cliente
                {
                    Id = int.Parse(txtId.Text),
                    Nombre = txtNombre.Text,
                    Correo = txtCorreo.Text,
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    Estado = chkEstado.Checked,
                    UsuarioModificacion = Login.usuarioActual,
                    FechaModificacion = DateTime.Now,
                    Usuario= new Usuario { Id=1, Username = txtUsuario.Text, ClaveAcceso = txtClaveAcceso.Text, }


                };

                bl.ActualizarCliente(c);

                CargarClientes();
                LimpiarCampos();

                MessageBox.Show("Cliente actualizado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Text);

                if (id == 1) {
                    MessageBox.Show("No se puede borrar el admin","Alerta",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                            
                bl.EliminarCliente(id);

                CargarClientes();
                LimpiarCampos();

                MessageBox.Show("Cliente eliminado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = bl.BuscarClientes(txtNombre.Text);
        }

        //Logica
        private void CargarClientes()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = bl.ObtenerClientes();
            txtNombre.Focus();
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtUsuario.Clear();
            txtClaveAcceso.Clear();
            chkEstado.Checked = false;
        }
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                filaSeleccionada = dgvClientes.Rows[e.RowIndex];

                txtId.Text = filaSeleccionada.Cells["Id"].Value.ToString();
                cliente = bl.ObtenerClientePorId(int.Parse(txtId.Text));

                txtNombre.Text = cliente.Nombre;
                txtCorreo.Text = cliente.Correo;
                txtTelefono.Text = cliente.Telefono;
                txtDireccion.Text = cliente.Direccion;
                chkEstado.Checked = cliente.Estado;
                txtUsuario.Text = cliente.Usuario.Username;
                txtClaveAcceso.Text = cliente.Usuario.ClaveAcceso;
            }
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }

        private void chkEstado_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
