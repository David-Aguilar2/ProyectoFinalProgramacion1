using BLL;
using DAL;
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
    // Formulario para la gestión (creación y edición) de productos en el almacén
    public partial class FrmAlmacen : Form
    {
        // Conexiones con la lógica de productos y categorías
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        // Variable para saber si estamos editando un producto existente o creando uno nuevo
        private Producto _productoEdicion;

        // El constructor puede recibir un producto (para editar) o nada (para agregar nuevo)
        public FrmAlmacen(Producto producto = null)
        {
            InitializeComponent();
            _productoEdicion = producto;
        }

        // Llena las listas desplegables (ComboBox) de Categorías y Estados
        private void CargarCombos()
        {
            var categorias = categoriaBLL.ObtenerCategorias();

            // Agrega una opción inicial por cortesía para que el usuario elija
            categorias.Insert(0, new Categoria
            {
                IdCategoria = 0,
                Nombre = "-- Seleccione una categoría --"
            });

            // Configura el combo de categorías para mostrar el Nombre pero usar el ID
            cmbCategoria.DataSource = categorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.SelectedIndex = 0;

            // Llena manualmente el combo de Estado (Activo/Inactivo)
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");
            cmbEstado.SelectedIndex = 0;
        }

        // Configura el formulario al abrirse
        private void FrmAlmacen_Load(object sender, EventArgs e)
        {
            CargarCombos();

            // Si se pasó un producto al abrir el formulario, cargamos sus datos en las cajas de texto
            if (_productoEdicion != null)
            {
                lblTitulo.Text = "Editar Producto";
                txtId.Text = _productoEdicion.IdProducto.ToString();
                txtNombre.Text = _productoEdicion.Nombre;
                txtPrecio.Value = _productoEdicion.Precio;
                txtStock.Value = _productoEdicion.Cantidad;
                cmbCategoria.SelectedValue = _productoEdicion.IdCategoria;
                cmbEstado.Text = _productoEdicion.Estado ? "Activo" : "Inactivo";
                Descripcion.Text = _productoEdicion.Descripcion;
                btnAceptar.Text = "Actualizar";
            }
            else
            {
                // Si no hay producto, preparamos el formulario para una nueva entrada
                lblTitulo.Text = "Nuevo Producto";
                btnAceptar.Text = "Agregar";
            }
        }

        // Evento que se dispara al presionar el botón de guardar/actualizar
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Recolectamos toda la información escrita en el formulario
            Producto p = new Producto
            {
                Nombre = txtNombre.Text,
                Descripcion = Descripcion.Text,
                Precio = txtPrecio.Value,
                Cantidad = (int)txtStock.Value,
                IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                Estado = cmbEstado.Text == "Activo"
            };

            // Obtenemos el ID del usuario que está usando el sistema actualmente
            int idUsuarioActual = GUI.Autenticacion.Login.UsuarioAutenticado.IdUsuario;

            string resultado;

            // Si _productoEdicion tiene datos, llamamos a la función de Actualizar
            if (_productoEdicion != null)
            {
                p.IdProducto = _productoEdicion.IdProducto;
                p.FechaRegistro = _productoEdicion.FechaRegistro;
                resultado = productoBLL.ActualizarProducto(p, idUsuarioActual);
            }
            else
            {
                // Si está vacío, llamamos a la función de Insertar (Nuevo)
                resultado = productoBLL.InsertarProducto(p);
            }

            // Si la respuesta de la lógica es "OK", avisamos al usuario y cerramos la ventana
            if (resultado == "OK")
            {
                MessageBox.Show("Operación realizada con éxito");
                this.Close();
            }
            else
            {
                // Si algo salió mal (ej. nombre repetido), mostramos el error específico
                MessageBox.Show("Error: " + resultado);
            }
        }
    }
}