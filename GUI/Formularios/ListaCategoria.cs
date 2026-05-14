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
    // Formulario que muestra la lista de categorías existentes en una tabla
    public partial class ListaCategoria : Form
    {
        // Conexión con la lógica de categorías
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        // Variable para saber si el usuario quiere volver atrás o cerrar la aplicación
        private bool regresandoAlMenu = false;

        public ListaCategoria()
        {
            InitializeComponent();
            ConfigurarGrid(); // Prepara la tabla al iniciar
        }

        // Configura las columnas y botones de la tabla (Grid)
        private void ConfigurarGrid()
        {
            // Limpia cualquier dato previo
            dgvCategorias.Rows.Clear();
            dgvCategorias.Columns.Clear();

            // Crea las columnas de información
            dgvCategorias.Columns.Add("Id", "ID");
            dgvCategorias.Columns.Add("Nombre", "Nombre");
            dgvCategorias.Columns.Add("Descripcion", "Descripción");

            // Agrega el botón de Editar a cada fila
            DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
            btnEditar.Name = "Editar";
            btnEditar.HeaderText = "Acción";
            btnEditar.Text = "Editar";
            btnEditar.UseColumnTextForButtonValue = true;
            dgvCategorias.Columns.Add(btnEditar);

            // Agrega el botón de Eliminar a cada fila
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Acción";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseColumnTextForButtonValue = true;
            dgvCategorias.Columns.Add(btnEliminar);

            // Carga los datos desde la base de datos
            CargarDatos("");

            // Ajustes visuales para que la tabla se vea bien
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.AllowUserToAddRows = false;
        }

        // Obtiene las categorías y las muestra en la tabla, permitiendo filtrar por ID
        private void CargarDatos(string filtroId)
        {
            dgvCategorias.Rows.Clear();
            var categorias = categoriaBLL.ObtenerCategorias();

            // Si el buscador tiene texto, filtra la lista; si no, muestra todas
            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? categorias
                : categorias.Where(c => c.IdCategoria.ToString().Contains(filtroId)).ToList();

            // Agrega cada categoría encontrada como una fila nueva
            listaFiltrada.ForEach(c =>
            {
                dgvCategorias.Rows.Add(c.IdCategoria, c.Nombre, c.Descripcion);
            });
        }

        // Se activa cada vez que escribes en el buscador para actualizar la tabla al instante
        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        // Detecta cuando haces clic en los botones de "Editar" o "Eliminar" dentro de la tabla
        private void dgvCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita errores si se hace clic en el encabezado
            if (e.RowIndex < 0) return;

            // Obtiene el ID de la fila seleccionada
            int id = Convert.ToInt32(dgvCategorias.Rows[e.RowIndex].Cells["Id"].Value);

            // Lógica para borrar
            if (dgvCategorias.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                DialogResult result = MessageBox.Show("¿Estas seguro que deseas eliminarlo?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string res = categoriaBLL.EliminarCategoria(id);
                    if (res == "OK") CargarDatos(""); // Refresca la tabla si se borró con éxito
                    else MessageBox.Show(res); // Muestra el error si no se pudo borrar
                }
            }

            // Lógica para editar
            if (dgvCategorias.Columns[e.ColumnIndex].Name == "Editar")
            {
                var categoria = categoriaBLL.ObtenerCategoriaPorId(id);
                FrmCategoria frm = new FrmCategoria(categoria);
                // Cuando se cierre la ventana de edición, actualiza la tabla
                frm.FormClosed += (s, args) => CargarDatos("");
                frm.ShowDialog();
            }
        }

        // Abre el formulario para crear una categoría nueva
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmCategoria frm = new FrmCategoria();
            frm.FormClosed += (s, args) => CargarDatos("");
            frm.ShowDialog();
        }

        // Cierra esta ventana para regresar a la lista de productos
        private void RGproductos_Click(object sender, EventArgs e)
        {
            regresandoAlMenu = true;
            this.Close();
        }

        // Controla el cierre total del programa si se presiona la "X" de la ventana
        private void ListaCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                    // Cierra toda la aplicación
                    Application.ExitThread();
                }
            }
        }
    }
}