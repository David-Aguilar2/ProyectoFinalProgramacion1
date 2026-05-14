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
using static System.Collections.Specialized.BitVector32;

namespace GUI
{
    // Formulario para visualizar y gestionar a todos los usuarios del sistema
    public partial class ListaUsuario : Form
    {
        // Conexión con la lógica de negocios de usuarios
        UsuarioBLL usuarioBLL = new UsuarioBLL();

        // Variable para distinguir entre volver al menú o cerrar la aplicación
        private bool regresandoAlMenu = false;

        public ListaUsuario()
        {
            InitializeComponent();
            ConfigurarGrid(); // Prepara la tabla al cargar el formulario
        }

        // Define las columnas de la tabla y los botones de acción (Editar/Eliminar)
        private void ConfigurarGrid()
        {
            dgvUsuarios.Rows.Clear();
            dgvUsuarios.Columns.Clear();

            // Columnas de información personal y de cuenta
            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("Nombre", "Nombre");
            dgvUsuarios.Columns.Add("Correo", "Correo");
            dgvUsuarios.Columns.Add("Usuario", "Usuario");
            dgvUsuarios.Columns.Add("Telefono", "Teléfono");
            dgvUsuarios.Columns.Add("Direccion", "Dirección");
            dgvUsuarios.Columns.Add("Estado", "Estado");

            // Botón para abrir el formulario de edición
            DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
            btnEditar.Name = "Editar";
            btnEditar.HeaderText = "Acción";
            btnEditar.Text = "Editar";
            btnEditar.UseColumnTextForButtonValue = true;
            dgvUsuarios.Columns.Add(btnEditar);

            // Botón para borrar el usuario de la base de datos
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Acción";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseColumnTextForButtonValue = true;
            dgvUsuarios.Columns.Add(btnEliminar);

            CargarDatos(""); // Llena la tabla inicialmente

            // Ajustes de diseño para que la tabla sea legible
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
        }

        // Carga la lista de usuarios aplicando filtros de búsqueda y de seguridad por rango
        private void CargarDatos(string filtroId)
        {
            dgvUsuarios.Rows.Clear();
            var usuarios = usuarioBLL.ObtenerUsuarios();

            // Filtra por ID si el usuario escribió algo en el buscador
            var listaFiltrada = string.IsNullOrEmpty(filtroId)
                ? usuarios
                : usuarios.Where(u => u.IdUsuario.ToString().Contains(filtroId)).ToList();

            // REGLA DE SEGURIDAD: Solo el SuperAdmin puede ver a otros SuperAdmins.
            // Si el usuario actual es un Administrador normal, no podrá ver al "dueño" en la lista.
            if (Login.UsuarioAutenticado.Rol != EL.Usuario.ROL_SUPERADMIN)
            {
                listaFiltrada = listaFiltrada.Where(u => u.Rol != EL.Usuario.ROL_SUPERADMIN).ToList();
            }

            // Agrega los usuarios permitidos a la tabla
            foreach (var u in listaFiltrada)
            {
                dgvUsuarios.Rows.Add(
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Username,
                    u.Telefono,
                    u.Direccion,
                    u.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        // Evento que actualiza la lista en tiempo real mientras se escribe el ID
        private void txtBuscarId_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscarId.Text);
        }

        // Controla los clics en los botones de "Editar" o "Eliminar" dentro de la tabla
        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idUsuario = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["Id"].Value);

            // Acción para EDITAR
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
            {
                Usuario usuario = usuarioBLL.ObtenerUsuarioPorId(idUsuario);
                FrmUsuarios frmUsuarios = new FrmUsuarios(usuario);
                // Al cerrar la ventana de edición, refresca la lista automáticamente
                frmUsuarios.FormClosed += (s, args) => CargarDatos(txtBuscarId.Text);
                frmUsuarios.ShowDialog();
            }
            // Acción para ELIMINAR
            else if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                var confirmResult = MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    usuarioBLL.EliminarUsuario(idUsuario);
                    CargarDatos(txtBuscarId.Text); // Refresca la lista después de borrar
                }
            }
        }

        // Abre el formulario para registrar un nuevo usuario
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmUsuarios frmUsuarios = new FrmUsuarios();
            frmUsuarios.FormClosed += (s, args) => CargarDatos(txtBuscarId.Text);
            frmUsuarios.ShowDialog();
        }

        // Cierra este formulario para volver al menú de navegación principal
        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            regresandoAlMenu = true;
            this.Close();
        }

        // Controla el cierre de la aplicación si el usuario presiona la "X"
        private void ListaUsuario_FormClosing(object sender, FormClosingEventArgs e)
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
                    e.Cancel = true; // Evita que el formulario se cierre
                }
                else
                {
                    // Cierra todos los hilos y la sesión de la aplicación
                    Application.ExitThread();
                }
            }
        }
    }
}