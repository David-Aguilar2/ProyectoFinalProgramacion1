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
        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            Descripcion.Clear();
            txtNombre.Focus();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Categoria nuevaCategoria = new Categoria
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text
            };

            string resultado = categoriaBLL.InsertarCategoria(nuevaCategoria);

            if (resultado == "OK")
            {
                MessageBox.Show("Categoría guardada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(resultado, "Error al agregar la categoría:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Debe buscar una categoría primero para poder actualizar.");
                return;
            }

            Categoria categoriaEditada = new Categoria
            {
                IdCategoria = Convert.ToInt32(txtId.Text),
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text
            };

            string resultado = categoriaBLL.ActualizarCategoria(categoriaEditada);

            if (resultado == "OK")
            {
                MessageBox.Show("¡Datos actualizados con éxito!");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Debe buscar una categoría primero para poder eliminar.");
                return;
            }
            int idCategoria = Convert.ToInt32(txtId.Text);
            string resultado = categoriaBLL.EliminarCategoria(idCategoria);
            if (resultado == "OK")
            {
                MessageBox.Show("¡Categoría eliminada con éxito!");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string idABuscar = Interaction.InputBox("Ingrese el ID de la categoría que desea buscar:", "Buscar categoría", "");

            if (!string.IsNullOrEmpty(idABuscar))
            {
                try
                {
                    int id = Convert.ToInt32(idABuscar);
                    var categoria = categoriaBLL.ObtenerCategoriaPorId(id);
                    if (categoria != null)
                    {
                        txtId.Text = categoria.IdCategoria.ToString();
                        txtNombre.Text = categoria.Nombre;
                        Descripcion.Text = categoria.Descripcion;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ninguna categoría con el ID: " + id, "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor, ingrese un número de ID válido.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
