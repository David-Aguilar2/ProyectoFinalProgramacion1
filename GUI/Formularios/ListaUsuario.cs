using BLL;
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

            dgvUsuarios.Rows.Add("1", "Ana Martínez", "ana@iconic.com", "7788-9900", "San Salvador", "Activo", "amartinez");
            dgvUsuarios.Rows.Add("2", "Carlos Pérez", "carlos@iconic.com", "2244-5566", "Santa Ana", "Activo", "cperez");
            dgvUsuarios.Rows.Add("3", "Elena Gómez", "elena@iconic.com", "7111-2233", "San Miguel", "Inactivo", "egomez");

            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
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
            this.Hide();
            FrmUsuarios usuario = new FrmUsuarios();
            usuario.ShowDialog();
            this.Show();
        }
    }
}
