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
    public partial class FrmAlmacen : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();
        private Producto _productoEdicion;
        public FrmAlmacen(Producto producto = null)
        {
            InitializeComponent();
            _productoEdicion = producto;
        }

        private void CargarCombos()
        {
            var categorias = categoriaBLL.ObtenerCategorias();

            categorias.Insert(0, new Categoria
            {
                IdCategoria = 0,
                Nombre = "-- Seleccione una categoría --"
            });

            cmbCategoria.DataSource = categorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";

            cmbCategoria.SelectedIndex = 0;

            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");
            cmbEstado.SelectedIndex = 0;
        }

        private void FrmAlmacen_Load(object sender, EventArgs e)
        {
            CargarCombos();

            if (_productoEdicion != null)
            {
                lblTitulo.Text = "Editar Producto";
                txtId.Text = _productoEdicion.IdProducto.ToString();
                txtNombre.Text = _productoEdicion.Nombre;
                txtPrecio.Value = _productoEdicion.Precio;
                txtStock.Value = _productoEdicion.Cantidad;
                cmbCategoria.SelectedValue = _productoEdicion.IdCategoria;
                cmbEstado.Text = _productoEdicion.Estado ? "Activo" : "Inactivo";
                Descripcion.Text = _productoEdicion.Descripcion;
            }
            else
            {
                lblTitulo.Text = "Nuevo Producto";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Producto p = new Producto
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text,
                Precio = txtPrecio.Value,
                Cantidad = (int)txtStock.Value,
                IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                Estado = cmbEstado.Text == "Activo"
            };

            string resultado;

            if (_productoEdicion != null)
            {
                p.IdProducto = _productoEdicion.IdProducto;
                p.FechaRegistro = _productoEdicion.FechaRegistro;
                resultado = productoBLL.ActualizarProducto(p);
            }
            else
            {
                // AGREGAR
                resultado = productoBLL.InsertarProducto(p);
            }

            if (resultado == "OK")
            {
                MessageBox.Show("Operación realizada con éxito");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: " + resultado);
            }
        }
    }
}
