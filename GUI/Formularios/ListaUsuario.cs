using BLL;
using DAL;
using EL;
using GUI.Autenticacion;
using GUI.Formularios;
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

namespace GUI
{
    public partial class ListaUsuario : Form
    {
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        public ListaUsuario()
        {
            InitializeComponent();
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvUsuarios.Rows.Clear();
            dgvUsuarios.Columns.Clear();
            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("Nombre", "Nombre");
            dgvUsuarios.Columns.Add("Correo", "Correo");
            dgvUsuarios.Columns.Add("Usuario", "Usuario");
            dgvUsuarios.Columns.Add("Telefono", "Teléfono");
            dgvUsuarios.Columns.Add("Direccion", "Dirección");
            dgvUsuarios.Columns.Add("Estado", "Estado");
            DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
            btnEditar.Name = "Editar";
            btnEditar.HeaderText = "Acción";
            btnEditar.Text = "Editar";
            btnEditar.UseColumnTextForButtonValue = true;
            dgvUsuarios.Columns.Add(btnEditar);
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Acción";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseColumnTextForButtonValue = true;
            dgvUsuarios.Columns.Add(btnEliminar);
            CargarDatos("");
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
        }

        private void CargarDatos(string filtroId)
        {
            dgvUsuarios.Rows.Clear();
            var usuarios = usuarioBLL.ObtenerUsuarios();
            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? usuarios
                : usuarios.Where(u => u.IdUsuario.ToString().Contains(filtroId)).ToList();
            foreach (var u in listaFiltrada)
            {
                dgvUsuarios.Rows.Add(u.IdUsuario, u.Nombre, u.Correo, u.Username, u.Telefono, u.Direccion, u.Estado ? "Activo" : "Inactivo");
            }
        }

        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int idUsuario = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["Id"].Value);
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
            {
                Usuario usuario = usuarioBLL.ObtenerUsuarioPorId(idUsuario);
                FrmUsuarios frmUsuarios = new FrmUsuarios(usuario);
                frmUsuarios.FormClosed += (s, args) => CargarDatos(txtBuscarId.Text);
                frmUsuarios.ShowDialog();
            }
            else if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                var confirmResult = MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    usuarioBLL.EliminarUsuario(idUsuario);
                    CargarDatos(txtBuscarId.Text);
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmUsuarios frmUsuarios = new FrmUsuarios();
            frmUsuarios.FormClosed += (s, args) => CargarDatos(txtBuscarId.Text);
            frmUsuarios.ShowDialog();
        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Evento de cierre del programa completo con confirmación
        private void ListaUsuario_FormClosing(object sender, FormClosingEventArgs e)
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
