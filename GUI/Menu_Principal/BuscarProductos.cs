using GUI.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Menu_Principal
{
    public partial class BuscarProductos : Form
    {
        //private Panel panelMenu;
        //private Panel panelDashboard;
        //private DataGridView dgvProductos;
        //private TextBox txtBuscar;
        //private Button btnVenta, btnAlmacen, btnUsuarios, btnNuevaVenta;

        public BuscarProductos()
        {
            InitializeComponent();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BuscarProductos_Load(object sender, EventArgs e)
        {

        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<ListaUsuario>();
        }
        private void btnAlmacen_Click(object sender, EventArgs e)
        {
            AbrirFormularioUnico<ListaAlmacen>();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbxResumen_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
