using BLL;
using DAL;
using EL;
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
            var userLogueado = Autenticacion.Login.UsuarioAutenticado;

            dgvAlmacen.Rows.Clear();
            dgvAlmacen.Columns.Clear();

            dgvAlmacen.Columns.Add("Id", "ID");
            dgvAlmacen.Columns.Add("Nombre", "Nombre");
            dgvAlmacen.Columns.Add("Precio", "Precio");
            dgvAlmacen.Columns.Add("Stock", "Stock");
            dgvAlmacen.Columns.Add("Categoria", "Categoría");
            dgvAlmacen.Columns.Add("Estado", "Estado");

            CargarDatos("");

            if (userLogueado.Rol != Usuario.ROL_TRABAJADOR)
            {
                DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
                btnEditar.Name = "Editar";
                btnEditar.HeaderText = "Acción";
                btnEditar.Text = "Editar";
                btnEditar.UseColumnTextForButtonValue = true;
                dgvAlmacen.Columns.Add(btnEditar);

                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "Acción";
                btnEliminar.Text = "Eliminar";
                btnEliminar.UseColumnTextForButtonValue = true;
                dgvAlmacen.Columns.Add(btnEliminar);
            }

            if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
            {
                agregar.Visible = false;
                gCategorias.Visible = false;
            }

            dgvAlmacen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlmacen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlmacen.AllowUserToAddRows = false;
        }

        private void CargarDatos(string filtroId)
        {
            dgvAlmacen.Rows.Clear();
            var listaCategorias = categoriaBLL.ObtenerCategorias();
            var productos = productoBLL.ObtenerProductos();

            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? productos
                : productos.Where(p => p.IdProducto.ToString().Contains(filtroId)).ToList();

            listaFiltrada.ForEach(p =>
            {
                var categoria = listaCategorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria);
                string nombreCategoria = categoria?.Nombre ?? "Sin Categoría";
                string estadoTexto = p.Estado ? "Activo" : "Inactivo";
                dgvAlmacen.Rows.Add(p.IdProducto, p.Nombre, p.Precio, p.Cantidad, nombreCategoria, estadoTexto);
            });
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

        private void dgvAlmacen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvAlmacen.Rows[e.RowIndex].Cells["Id"].Value);

            if (dgvAlmacen.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                DialogResult result = MessageBox.Show("¿Estás seguro que deseas eliminarlo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    productoBLL.EliminarProducto(id);
                    CargarDatos("");
                }
            }

            if (dgvAlmacen.Columns[e.ColumnIndex].Name == "Editar")
            {
                var producto = productoBLL.ObtenerProductoPorId(id);
                FrmAlmacen frm = new FrmAlmacen(producto);
                frm.FormClosed += (s, args) => CargarDatos("");
                frm.ShowDialog();
            }
        }

        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void agregar_Click(object sender, EventArgs e)
        {
            FrmAlmacen frm = new FrmAlmacen();
            frm.FormClosed += (s, args) => CargarDatos("");
            frm.ShowDialog();
        }

        private void gCategorias_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<ListaCategoria>();
        }

        // Evento de cierre del programa completo con confirmación
        private void ListaAlmacen_FormClosing(object sender, FormClosingEventArgs e)
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