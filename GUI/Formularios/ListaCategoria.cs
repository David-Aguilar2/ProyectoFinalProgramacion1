using GUI.Menu_Principal;
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

namespace GUI.Formularios
{
    public partial class ListaCategoria : Form
    {
        CategoriaBLL categoriaBLL = new CategoriaBLL();
        public ListaCategoria()
        {
            InitializeComponent();
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvCategorias.Rows.Clear();
            dgvCategorias.Columns.Clear();

            dgvCategorias.Columns.Add("Id", "ID");
            dgvCategorias.Columns.Add("Nombre", "Nombre");
            dgvCategorias.Columns.Add("Descripcion", "Descripción");

            categoriaBLL.ObtenerCategorias().ForEach(c =>
            {
                dgvCategorias.Rows.Add(c.IdCategoria, c.Nombre, c.Descripcion);
            });

            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.AllowUserToAddRows = false;
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

        private void crud_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<FrmCategoria>();
        }

        private void RGproductos_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
