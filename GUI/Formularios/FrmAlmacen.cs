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
        // Instancia global de la lógica de negocio
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();
        Producto productoEncontrado;
        public FrmAlmacen()
        {
            InitializeComponent();
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtPrecio.Value = 0;
            txtStock.Value = 0;
            cmbCategoria.SelectedIndex = 0;
            cmbEstado.SelectedIndex = 0;
            Descripcion.Clear();
            productoEncontrado = null;
            txtNombre.Focus();

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
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Producto nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text,
                Precio = txtPrecio.Value,
                Cantidad = (int)txtStock.Value,
                Estado = cmbEstado.Text == "Activo",
                IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue)
            };

            string resultado = productoBLL.InsertarProducto(nuevoProducto);

            if (resultado == "OK")
            {
                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al agregar el producto: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Debe buscar un producto primero para poder actualizar.");
                    return;
                }
    
                Producto productoActualizado = new Producto
                {
                    IdProducto = Convert.ToInt32(txtId.Text),
                    Nombre = txtNombre.Text,
                    Descripcion = Descripcion.Text,
                    Precio = txtPrecio.Value,
                    Cantidad = (int)txtStock.Value,
                    Estado = cmbEstado.Text == "Activo",
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    FechaRegistro = productoEncontrado.FechaRegistro
                };
    
                string resultado = productoBLL.ActualizarProducto(productoActualizado);
    
                if (resultado == "OK")
                {
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el producto: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Debe buscar un producto primero para poder eliminar.");
                return;
            }
            int idProducto = int.Parse(txtId.Text);
            string resultado = productoBLL.EliminarProducto(idProducto);
            if (resultado == "OK")
            {
                MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al eliminar el producto: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string idABuscar = Interaction.InputBox("Ingrese el ID del producto que desea buscar:", "Buscar producto", "");

            if (!string.IsNullOrEmpty(idABuscar))
            {
                try
                {
                    int id = Convert.ToInt32(idABuscar);

                    productoEncontrado = productoBLL.ObtenerProductoPorId(id);
                    if (productoEncontrado != null)
                    {
                        txtId.Text = productoEncontrado.IdProducto.ToString();
                        txtNombre.Text = productoEncontrado.Nombre;
                        Descripcion.Text = productoEncontrado.Descripcion;
                        txtPrecio.Value = productoEncontrado.Precio;
                        txtStock.Value = productoEncontrado.Cantidad;
                        cmbEstado.Text = productoEncontrado.Estado ? "Activo" : "Inactivo";
                        cmbCategoria.SelectedValue = productoEncontrado.IdCategoria;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún producto con el ID: " + id, "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
