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

namespace GUI.Formularios
{
    public partial class ListaCategoria : Form
    {
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

            dgvCategorias.Rows.Add("1", "Camisas", "Prendas superiores formales y casuales");
            dgvCategorias.Rows.Add("2", "Pantalones", "Cortes chino, slim y denim para toda ocasión");
            dgvCategorias.Rows.Add("3", "Vestidos", "Colección de gala, noche y vestidos de verano");

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
