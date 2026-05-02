using BLL;
using DAL;
using GUI.Formularios;
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
    public partial class ListaAlmacen : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        public ListaAlmacen()
        {
            InitializeComponent();
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvAlmacen.Rows.Clear();
            dgvAlmacen.Columns.Clear();

            dgvAlmacen.Columns.Add("Id", "ID");
            dgvAlmacen.Columns.Add("Nombre", "Nombre");
            dgvAlmacen.Columns.Add("Precio", "Precio");
            dgvAlmacen.Columns.Add("Stock", "Stock");
            dgvAlmacen.Columns.Add("Categoria", "Categoría");
            dgvAlmacen.Columns.Add("Descripcion", "Descripción");

            var listaCategorias = categoriaBLL.ObtenerCategorias();

            productoBLL.ObtenerProductos().ForEach(p =>
            {
                var categoria = listaCategorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria);
                string nombreCategoria = (categoria != null) ? categoria.Nombre : "Sin Categoría";

                dgvAlmacen.Rows.Add(p.IdProducto, p.Nombre, p.Precio, p.Cantidad, nombreCategoria, p.Descripcion);
            });

            dgvAlmacen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlmacen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlmacen.AllowUserToAddRows = false;
        }

        private void AbrirFormularioUnico<T>() where T : Form, new()
        {
            Form formularioExistente = Application.OpenForms.OfType<T>().FirstOrDefault();

            if (formularioExistente != null)
            {
                formularioExistente.Close();
            }

            T nuevoFormulario = new T();

            nuevoFormulario.FormClosed += (s, args) => this.Show();

            this.Hide();
            nuevoFormulario.Show();
        }

        private void dgvAlmacen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void crud_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<FrmAlmacen>();
        }

        private void gCategorias_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<ListaCategoria>();
        }
    }
}