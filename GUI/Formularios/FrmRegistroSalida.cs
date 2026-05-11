using BLL;
using EL;
using GUI.Autenticacion;
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
    public partial class FrmRegistroSalida : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        RegistroSalidaBLL registroBLL = new RegistroSalidaBLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public FrmRegistroSalida()
        {
            InitializeComponent();
        }

        private void FrmRegistroSalida_Load(object sender, EventArgs e)
        {
            CargarCombos();

            txtId.Enabled = false;

            if (Login.UsuarioAutenticado != null)
            {
                cmbUsuario.SelectedValue = Login.UsuarioAutenticado.IdUsuario;
                cmbUsuario.Enabled = false; 
            }

            lblTitulo.Text = "Movimiento de Inventario";
            btnAceptar.Text = "Aceptar";
        }

        private void CargarCombos()
        {
            var productos = productoBLL.ObtenerProductos();
            cmbProducto.DataSource = productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "IdProducto";

            var usuarios = usuarioBLL.ObtenerUsuarios();
            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "IdUsuario";

            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("ENTRADA");
            cmbTipo.Items.Add("SALIDA");
            cmbTipo.SelectedIndex = 1;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un producto.");
                return;
            }

            if (txtCantidad.Value <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Debe indicar un motivo para el movimiento.");
                return;
            }

            RegistroSalida movimiento = new RegistroSalida
            {
                IdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
                IdUsuario = Login.UsuarioAutenticado.IdUsuario,
                Tipo = cmbTipo.Text,
                Cantidad = (int)txtCantidad.Value,
                Motivo = txtMotivo.Text,
                FechaSalida = DateTime.Now
            };

            string resultado = registroBLL.InsertarMovimiento(movimiento);

            if (resultado == "OK")
            {
                MessageBox.Show("Movimiento registrado y stock actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al registrar: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
