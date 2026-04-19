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
        public BuscarProductos()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void BuscarProductos_Load(object sender, EventArgs e)
        {

        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            this.Hide();
            ListaUsuario usuario = new ListaUsuario();
            usuario.ShowDialog();
            this.Show();
        }
        private void btnAlmacen_Click(object sender, EventArgs e)
        {
            this.Hide();
            ListaAlmacen almacen = new ListaAlmacen();
            almacen.ShowDialog();
            this.Show();
        }
    }
}
