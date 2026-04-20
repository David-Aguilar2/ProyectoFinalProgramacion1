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

            dgvAlmacen.Rows.Add("1", "Camisa Oxford Slim", "25.99", "15", "Camisas", "Camisa de algodón premium color azul");
            dgvAlmacen.Rows.Add("2", "Pantalón Chino Beige", "35.50", "8", "Pantalones", "Corte recto, ideal para oficina");
            dgvAlmacen.Rows.Add("3", "Vestido Gala Noche", "85.00", "3", "Vestidos", "Vestido largo fucsia con detalles en seda");

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