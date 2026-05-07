using BLL;
using EL;
using Microsoft.VisualBasic;
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
    public partial class FrmCategoria : Form
    {
        // Instancia global de la lógica de negocio
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        private Categoria _categoriaEdicion;
        public FrmCategoria(Categoria categoria = null)
        {
            InitializeComponent();
            _categoriaEdicion = categoria;
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            if (_categoriaEdicion != null)
            {
                lblTitulo.Text = "Editar Categoría";
                txtId.Text = _categoriaEdicion.IdCategoria.ToString();
                txtNombre.Text = _categoriaEdicion.Nombre;
                Descripcion.Text = _categoriaEdicion.Descripcion;
                btnAceptar.Text = "Actualizar";
            }
            else
            {
                lblTitulo.Text = "Nueva Categoría";
                btnAceptar.Text = "Guardar";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            Categoria c = new Categoria
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text
            };

            string resultado;

            if (_categoriaEdicion != null)
            {
                c.IdCategoria = _categoriaEdicion.IdCategoria;
                resultado = categoriaBLL.ActualizarCategoria(c);
            }
            else
            {
                resultado = categoriaBLL.InsertarCategoria(c);
            }

            if (resultado == "OK")
            {
                MessageBox.Show("Operación realizada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
