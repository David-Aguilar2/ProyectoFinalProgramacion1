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
        public ListaCategoria()
        {
            InitializeComponent();
        }

        private void crud_Click(object sender, EventArgs e)
        {
            this.Close();
            FrmCategoria usuario = new FrmCategoria();
            usuario.ShowDialog();
            this.Show();
        }

        private void RGproductos_Click(object sender, EventArgs e)
        {
            this.Close();
            ListaAlmacen usuario = new ListaAlmacen();
            usuario.ShowDialog();
            this.Show();
        }
    }
}
