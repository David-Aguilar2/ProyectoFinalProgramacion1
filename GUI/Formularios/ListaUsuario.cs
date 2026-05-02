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
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            CargarDatosDePrueba();
        }

        private void CargarDatosDePrueba()
        {
            dgvUsuarios.Rows.Clear();
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("Nombre", "Nombre");
            dgvUsuarios.Columns.Add("Correo", "Correo");
            dgvUsuarios.Columns.Add("Telefono", "Teléfono");
            dgvUsuarios.Columns.Add("Direccion", "Dirección");
            dgvUsuarios.Columns.Add("Estado", "Estado");
            dgvUsuarios.Columns.Add("Usuario", "Usuario");

            usuarioBLL.ObtenerUsuarios().ForEach(u =>
            {
                string estado = u.Estado ? "Activo" : "Inactivo";
                dgvUsuarios.Rows.Add(u.IdUsuario, u.Nombre, u.Correo, u.Telefono, u.Direccion, estado, u.Username);
            });

            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
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
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RMPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void crud_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<FrmUsuarios>();
        }
    }
}
