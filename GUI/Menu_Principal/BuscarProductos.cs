using BLL;
using EL;
using GUI.Autenticacion;
using GUI.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Menu_Principal
{
    public partial class BuscarProductos : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        public BuscarProductos()
        {
            InitializeComponent();
        }

        private void ConfigurarGrid()
        {
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.ReadOnly = true;
            CargarDatos("");
        }

        private void CargarDatos(string filtro)
        {
            var productos = productoBLL.ObtenerProductos();
            var categorias = categoriaBLL.ObtenerCategorias();

            var listaFiltrada = productos
                .Where(p => p.Estado == true &&
                           (p.Nombre.ToLower().Contains(filtro.ToLower()) ||
                            p.Descripcion.ToLower().Contains(filtro.ToLower())))
                .Select(p => new
                {
                    p.IdProducto,
                    p.Nombre,
                    Categoria = categorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria)?.Nombre ?? "N/A",
                    p.Cantidad,
                    p.Precio
                }).ToList();

            dgvProductos.DataSource = listaFiltrada;
        }

        private void ActualizarDashboard()
        {
            var productos = productoBLL.ObtenerProductos();

            int totalStock = productos.Count(p => p.Cantidad > 0);
            int stockBajo = productos.Count(p => p.Cantidad < 5);

            lblStock.Text = $"{totalStock}";
            lblStockBajo.Text = $"{stockBajo}";
            lblVentasHoy.Text = $"$0.00";
        }


        private void AbrirFormularioUnico<T>() where T : Form, new()
        {
            Form formularioExistente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (formularioExistente != null) formularioExistente.Close();

            T nuevoFormulario = new T();
            nuevoFormulario.FormClosed += (s, args) => {
                this.Show();
                ActualizarDashboard();
                CargarDatos("");
            };

            this.Hide();
            nuevoFormulario.Show();
        }

        private void BuscarProductos_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarGrid();
                ActualizarDashboard();
                AplicarPermisos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void AplicarPermisos()
        {
            var userLogueado = Login.UsuarioAutenticado;

            if (userLogueado != null)
            {
                if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
                {
                    btnUsuarios.Visible = false;

                }

                if (userLogueado.Rol == Usuario.ROL_ADMIN)
                {
                    btnUsuarios.Visible = true;
                    btnAlmacen.Visible = true;
                }

            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e) => AbrirFormularioUnico<ListaUsuario>();
        private void btnAlmacen_Click(object sender, EventArgs e) => AbrirFormularioUnico<ListaAlmacen>();

        private void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text);
        }

        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                MessageBox.Show("Abriendo formulario de venta para: " + dgvProductos.SelectedRows[0].Cells["Nombre"].Value);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto de la lista.");
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
        "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                GUI.Autenticacion.Login.UsuarioAutenticado = null;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void lblBuscar_Click(object sender, EventArgs e)
        {

        }
    }
}
