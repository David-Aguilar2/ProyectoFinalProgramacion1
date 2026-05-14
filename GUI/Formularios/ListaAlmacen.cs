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
    // Formulario que muestra la tabla con todos los productos del inventario
    public partial class ListaAlmacen : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        // Variable para saber si cerramos la ventana para ir al menú o para salir del programa
        private bool regresandoAlMenu = false;

        public ListaAlmacen()
        {
            InitializeComponent();
            ConfigurarGrid(); // Prepara la tabla al abrir la ventana
        }

        // Configura cómo se ve la tabla y qué permisos tiene el usuario
        private void ConfigurarGrid()
        {
            var userLogueado = Autenticacion.Login.UsuarioAutenticado;

            // Limpia la tabla y crea las columnas de información básica
            dgvAlmacen.Rows.Clear();
            dgvAlmacen.Columns.Clear();

            dgvAlmacen.Columns.Add("Id", "ID");
            dgvAlmacen.Columns.Add("Nombre", "Nombre");
            dgvAlmacen.Columns.Add("Precio", "Precio");
            dgvAlmacen.Columns.Add("Stock", "Stock");
            dgvAlmacen.Columns.Add("Categoria", "Categoría");
            dgvAlmacen.Columns.Add("Estado", "Estado");

            CargarDatos(""); // Llena la tabla con los productos

            // SEGURIDAD: Si no es un trabajador (es Admin), le muestra botones de Editar y Eliminar
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

            // SEGURIDAD: Si es trabajador, le esconde los botones de agregar y gestionar categorías
            if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
            {
                agregar.Visible = false;
                gCategorias.Visible = false;
            }

            // Ajustes visuales de la tabla
            dgvAlmacen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlmacen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlmacen.AllowUserToAddRows = false;
        }

        // Busca los productos y los pone dentro de la tabla
        private void CargarDatos(string filtroId)
        {
            dgvAlmacen.Rows.Clear();
            var listaCategorias = categoriaBLL.ObtenerCategorias();
            var productos = productoBLL.ObtenerProductos();

            // Si el usuario escribió algo en el buscador, filtramos la lista por ID
            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? productos
                : productos.Where(p => p.IdProducto.ToString().Contains(filtroId)).ToList();

            // Por cada producto encontrado, agregamos una fila a la tabla
            listaFiltrada.ForEach(p =>
            {
                var categoria = listaCategorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria);
                string nombreCategoria = categoria?.Nombre ?? "Sin Categoría";
                string estadoTexto = p.Estado ? "Activo" : "Inactivo";
                dgvAlmacen.Rows.Add(p.IdProducto, p.Nombre, p.Precio, p.Cantidad, nombreCategoria, estadoTexto);
            });
        }

        // Función auxiliar para abrir una ventana nueva y ocultar la actual
        private void AbrirFormularioUnico<T>() where T : Form, new()
        {
            Form formularioExistente = Application.OpenForms.OfType<T>().FirstOrDefault();

            if (formularioExistente != null)
            {
                formularioExistente.Close();
            }

            T nuevoFormulario = new T();
            // Cuando se cierre la ventana nueva, volvemos a mostrar esta lista
            nuevoFormulario.FormClosed += (s, args) => this.Show();

            this.Hide();
            nuevoFormulario.Show();
        }

        // Se ejecuta al hacer clic en los botones de "Editar" o "Eliminar" dentro de la tabla
        private void dgvAlmacen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvAlmacen.Rows[e.RowIndex].Cells["Id"].Value);

            // Acción para el botón Eliminar
            if (dgvAlmacen.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                DialogResult result = MessageBox.Show("¿Estás seguro que deseas eliminarlo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    productoBLL.EliminarProducto(id);
                    CargarDatos(""); // Refresca la tabla
                }
            }

            // Acción para el botón Editar
            if (dgvAlmacen.Columns[e.ColumnIndex].Name == "Editar")
            {
                var producto = productoBLL.ObtenerProductoPorId(id);
                FrmAlmacen frm = new FrmAlmacen(producto);
                // Al cerrar la edición, refresca los datos de la tabla
                frm.FormClosed += (s, args) => CargarDatos("");
                frm.ShowDialog();
            }
        }

        // Actualiza la tabla automáticamente mientras el usuario escribe en el buscador
        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        // Botón para cerrar esta ventana y volver al menú principal
        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            regresandoAlMenu = true;
            this.Close();
        }

        // Botón para abrir el formulario de agregar un producto nuevo
        private void agregar_Click(object sender, EventArgs e)
        {
            FrmAlmacen frm = new FrmAlmacen();
            frm.FormClosed += (s, args) => CargarDatos("");
            frm.ShowDialog();
        }

        // Botón para ir a la gestión de categorías
        private void gCategorias_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<ListaCategoria>();
        }

        // Controla qué pasa cuando se intenta cerrar la ventana (por la X o por código)
        private void ListaAlmacen_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si no estamos regresando al menú (es decir, el usuario cerró la app), pide confirmación
            if (!regresandoAlMenu)
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Estás seguro de querer salir? Se cerrará la sesión actual",
                    "Confirmar Salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (resultado == DialogResult.No)
                {
                    e.Cancel = true; // No cierra la ventana
                }
                else
                {
                    // Si confirma, cierra toda la aplicación por completo
                    Application.ExitThread();
                }
            }
        }
    }
}