using GUI.Menu_Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmAlmacen : Form
    {
        // Estructura interna para no depender de archivos externos (EL)
        public class ProductoAlmacen
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Precio { get; set; }
            public string Stock { get; set; }
            public string Categoria { get; set; }
            public string Descripcion { get; set; }
        }

        // Esto es para datos temporales
        private BindingList<ProductoAlmacen> listaAlmacen = new BindingList<ProductoAlmacen>();

        public FrmAlmacen()
        {
            InitializeComponent();
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvAlmacen.DataSource = listaAlmacen;
            dgvAlmacen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlmacen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlmacen.ReadOnly = true;
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Nombre.Text)) return;

            int nuevoId = listaAlmacen.Any() ? listaAlmacen.Max(x => x.Id) + 1 : 1;

            ProductoAlmacen nuevo = new ProductoAlmacen
            {
                Id = nuevoId,
                Nombre = Nombre.Text,
                Precio = Precio.Text,
                Stock = Stock.Text,
                Categoria = Categoria.Text,
                Descripcion = Descripcion.Text
            };

            listaAlmacen.Add(nuevo);
            LimpiarCampos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ID.Text)) return;

            int idBusca = int.Parse(ID.Text);
            var item = listaAlmacen.FirstOrDefault(x => x.Id == idBusca);

            if (item != null)
            {
                item.Nombre = Nombre.Text;
                item.Precio = Precio.Text;
                item.Stock = Stock.Text;
                item.Categoria = Categoria.Text;
                item.Descripcion = Descripcion.Text;

                listaAlmacen.ResetBindings();
                MessageBox.Show("Producto actualizado en el almacén.");
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ID.Text)) return;

            int idEliminar = int.Parse(ID.Text);
            var item = listaAlmacen.FirstOrDefault(x => x.Id == idEliminar);

            if (item != null)
            {
                listaAlmacen.Remove(item);
                LimpiarCampos();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = Nombre.Text.ToLower();
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                dgvAlmacen.DataSource = listaAlmacen;
            }
            else
            {
                var filtrados = listaAlmacen.Where(x => x.Nombre.ToLower().Contains(busqueda)).ToList();
                dgvAlmacen.DataSource = new BindingList<ProductoAlmacen>(filtrados);
            }
        }

        private void LimpiarCampos()
        {
            ID.Clear();
            Nombre.Clear();
            Precio.Clear();
            Stock.Clear();
            Categoria.Clear();
            Descripcion.Clear();
            Nombre.Focus();
        }

        private void dgvAlmacen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvAlmacen.Rows[e.RowIndex];

                ID.Text = fila.Cells["Id"].Value?.ToString();
                Nombre.Text = fila.Cells["Nombre"].Value?.ToString();
                Precio.Text = fila.Cells["Precio"].Value?.ToString();
                Stock.Text = fila.Cells["Stock"].Value?.ToString();
                Categoria.Text = fila.Cells["Categoria"].Value?.ToString();
                Descripcion.Text = fila.Cells["Descripcion"].Value?.ToString();
            }
        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}