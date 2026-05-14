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

            DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
            btnEditar.Name = "Editar";
            btnEditar.HeaderText = "Acción";
            btnEditar.Text = "Editar";
            btnEditar.UseColumnTextForButtonValue = true;
            dgvCategorias.Columns.Add(btnEditar);

            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Acción";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseColumnTextForButtonValue = true;
            dgvCategorias.Columns.Add(btnEliminar);

            CargarDatos("");

            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.AllowUserToAddRows = false;
        }

        private void CargarDatos(string filtroId)
        {
            dgvCategorias.Rows.Clear();
            var categorias = categoriaBLL.ObtenerCategorias();

            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? categorias
                : categorias.Where(c => c.IdCategoria.ToString().Contains(filtroId)).ToList();

            listaFiltrada.ForEach(c =>
            {
                dgvCategorias.Rows.Add(c.IdCategoria, c.Nombre, c.Descripcion);
            });
        }

        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        private void dgvCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvCategorias.Rows[e.RowIndex].Cells["Id"].Value);

            if (dgvCategorias.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                DialogResult result = MessageBox.Show("¿Estas seguro que deseas eliminarlo?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string res = categoriaBLL.EliminarCategoria(id);
                    if (res == "OK") CargarDatos("");
                    else MessageBox.Show(res);
                }
            }

            if (dgvCategorias.Columns[e.ColumnIndex].Name == "Editar")
            {
                var categoria = categoriaBLL.ObtenerCategoriaPorId(id);
                FrmCategoria frm = new FrmCategoria(categoria);
                frm.FormClosed += (s, args) => CargarDatos("");
                frm.ShowDialog();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmCategoria frm = new FrmCategoria();
            frm.FormClosed += (s, args) => CargarDatos("");
            frm.ShowDialog();
        }

        private void RGproductos_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Evento de cierre del programa completo con confirmación
        private void ListaCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cuadro de diálogo de confirmación
            DialogResult resultado = MessageBox.Show(
                "¿Estás seguro de querer salir? Se cerrará la sesión actual",
                "Confirmar Salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Si el usuario presiona "No", cancelamos el cierre
            if (resultado == DialogResult.No)
            {
                e.Cancel = true; // Esto detiene el cierre del formulario
            }
            else
            {
                // Si dice que "Sí", cerramos toda la aplicación por completo
                Application.ExitThread();
            }
        }
    }
}
