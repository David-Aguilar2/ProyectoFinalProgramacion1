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
    // Formulario para registrar manualmente cuando entra o sale mercancía
    public partial class FrmRegistroSalida : Form
    {
        // Conexiones con las capas que manejan productos, movimientos y usuarios
        ProductoBLL productoBLL = new ProductoBLL();
        RegistroSalidaBLL registroBLL = new RegistroSalidaBLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        public FrmRegistroSalida()
        {
            InitializeComponent();
        }

        // Configuración inicial al abrir la ventana de movimientos
        private void FrmRegistroSalida_Load(object sender, EventArgs e)
        {
            CargarCombos(); // Llena las listas de productos y usuarios

            txtId.Enabled = false; // El ID no se toca porque es automático

            // Bloquea el selector de usuario para que aparezca el nombre de quien inició sesión
            if (Login.UsuarioAutenticado != null)
            {
                cmbUsuario.SelectedValue = Login.UsuarioAutenticado.IdUsuario;
                cmbUsuario.Enabled = false;
            }

            lblTitulo.Text = "Movimiento de Inventario";
            btnAceptar.Text = "Aceptar";
        }

        // Carga los datos necesarios en las listas desplegables
        private void CargarCombos()
        {
            // Llena la lista de productos disponibles
            var productos = productoBLL.ObtenerProductos();
            cmbProducto.DataSource = productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "IdProducto";

            // Llena la lista de usuarios para el registro
            var usuarios = usuarioBLL.ObtenerUsuarios();
            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "IdUsuario";

            // Configura las opciones de tipo de movimiento
            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("ENTRADA");
            cmbTipo.Items.Add("SALIDA");
            cmbTipo.SelectedIndex = 1; // Por defecto selecciona "SALIDA"
        }

        // Evento que ocurre al confirmar el movimiento de inventario
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Validaciones rápidas antes de intentar guardar
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

            // Crea el objeto con los datos que el usuario llenó en pantalla
            RegistroSalida movimiento = new RegistroSalida
            {
                IdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
                IdUsuario = Login.UsuarioAutenticado.IdUsuario,
                Tipo = cmbTipo.Text,
                Cantidad = (int)txtCantidad.Value,
                Motivo = txtMotivo.Text,
                FechaSalida = DateTime.Now
            };

            // Envía el movimiento a la lógica para que descuente o sume del stock real
            string resultado = registroBLL.InsertarMovimiento(movimiento);

            if (resultado == "OK")
            {
                MessageBox.Show("Movimiento registrado y stock actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario si todo salió bien
            }
            else
            {
                // Si hubo un error (como intentar sacar más de lo que hay), muestra el aviso
                MessageBox.Show("Error al registrar: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}