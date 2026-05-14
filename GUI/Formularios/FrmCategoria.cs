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
    // Formulario para agregar nuevas categorías o modificar las existentes
    public partial class FrmCategoria : Form
    {
        // Conexión con la lógica de negocio para validar y guardar datos
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        // Variable para guardar temporalmente la categoría que queremos editar
        private Categoria _categoriaEdicion;

        // El constructor recibe una categoría si vamos a editar, o nada si es una nueva
        public FrmCategoria(Categoria categoria = null)
        {
            InitializeComponent();
            _categoriaEdicion = categoria;
        }

        // Se ejecuta cuando el formulario se abre
        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            // Si recibimos una categoría para editar, llenamos las cajas de texto con sus datos
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
                // Si no hay datos previos, preparamos el formulario para una categoría nueva
                lblTitulo.Text = "Nueva Categoría";
                btnAceptar.Text = "Guardar";
            }
        }

        // Se ejecuta al hacer clic en el botón Guardar o Actualizar
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Validación visual rápida para asegurar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            // Creamos un objeto categoría con lo que el usuario escribió
            Categoria c = new Categoria
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text
            };

            string resultado;

            // Si estamos editando, usamos el método de actualizar pasando el ID original
            if (_categoriaEdicion != null)
            {
                c.IdCategoria = _categoriaEdicion.IdCategoria;
                resultado = categoriaBLL.ActualizarCategoria(c);
            }
            else
            {
                // Si es nueva, usamos el método de insertar
                resultado = categoriaBLL.InsertarCategoria(c);
            }

            // Revisamos si la operación fue exitosa
            if (resultado == "OK")
            {
                MessageBox.Show("Operación realizada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cerramos la ventana después de guardar
            }
            else
            {
                // Si hubo un error (ej. nombre duplicado), lo mostramos en pantalla
                MessageBox.Show("Error: " + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}