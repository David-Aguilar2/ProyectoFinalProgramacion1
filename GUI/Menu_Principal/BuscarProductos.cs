using BLL;
using EL;
using GUI.Autenticacion;
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
    // Este es el formulario principal (Dashboard) que ve el usuario al entrar
    public partial class BuscarProductos : Form
    {
        ProductoBLL productoBLL = new ProductoBLL();
        CategoriaBLL categoriaBLL = new CategoriaBLL();

        private bool regresandoAlMenu = false;

        public BuscarProductos()
        {
            InitializeComponent();
        }

        // Configura las opciones básicas de la tabla de productos
        private void ConfigurarGrid()
        {
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.ReadOnly = true;
            CargarDatos(""); // Carga la lista inicialmente vacía (todos los productos)
        }

        // Busca productos activos y permite filtrar por nombre/descripción o por stock bajo
        private void CargarDatos(string filtro)
        {
            var productos = productoBLL.ObtenerProductos();
            var categorias = categoriaBLL.ObtenerCategorias();

            bool filtrarStockBajo = checkStockBajo.Checked;

            // Filtramos solo productos activos (Estado == true)
            var listaFiltrada = productos
                .Where(p => p.Estado == true &&
                           (p.Nombre.ToLower().Contains(filtro.ToLower()) ||
                            p.Descripcion.ToLower().Contains(filtro.ToLower())))
                // Si el checkbox de stock bajo está marcado, solo muestra los que tienen menos de 5 unidades
                .Where(p => !filtrarStockBajo || p.Cantidad < 5)
                .Select(p => new
                {
                    p.IdProducto,
                    p.Nombre,
                    // Buscamos el nombre de la categoría o ponemos N/A si no existe
                    Categoria = categorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria)?.Nombre ?? "N/A",
                    p.Cantidad,
                    p.Precio
                }).ToList();

            dgvProductos.DataSource = listaFiltrada;
        }

        // Actualiza los contadores visuales (tarjetas de información) del menú
        private void ActualizarDashboard()
        {
            var productos = productoBLL.ObtenerProductos();

            int totalStock = productos.Count(p => p.Cantidad > 0);
            int stockBajo = productos.Count(p => p.Cantidad < 5);

            lblStock.Text = $"{totalStock}";
            lblStockBajo.Text = $"{stockBajo}";
        }

        // Método genérico para abrir otros formularios ocultando el menú principal
        private void AbrirFormularioUnico<T>() where T : Form, new()
        {
            Form formularioExistente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (formularioExistente != null) formularioExistente.Close();

            T nuevoFormulario = new T();
            // Al cerrar el formulario abierto, volvemos a mostrar el menú y refrescamos los datos
            nuevoFormulario.FormClosed += (s, args) => {
                this.Show();
                ActualizarDashboard();
                CargarDatos("");
            };

            this.Hide();
            nuevoFormulario.Show();
        }

        private void BuscarProductos_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarGrid();
                ActualizarDashboard();
                AplicarPermisos(); // Ajusta qué botones puede ver el usuario según su rol
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        // REGLAS DE SEGURIDAD: Oculta o muestra botones según el Rol del usuario
        private void AplicarPermisos()
        {
            var userLogueado = Login.UsuarioAutenticado;

            if (userLogueado != null)
            {
                // El trabajador no puede gestionar usuarios
                if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
                {
                    btnUsuarios.Visible = false;
                }

                // El administrador y superadmin pueden ver almacén y usuarios
                if (userLogueado.Rol == Usuario.ROL_ADMIN || userLogueado.Rol == Usuario.ROL_SUPERADMIN)
                {
                    btnUsuarios.Visible = true;
                    btnAlmacen.Visible = true;
                }
            }
        }

        // Navegación a otros módulos
        private void btnUsuarios_Click(object sender, EventArgs e) => AbrirFormularioUnico<ListaUsuario>();
        private void btnAlmacen_Click(object sender, EventArgs e) => AbrirFormularioUnico<ListaAlmacen>();
        private void btnVenta_Click(object sender, EventArgs e) => AbrirFormularioUnico<ListaRegistroSalida>();

        // Filtro de búsqueda en tiempo real
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text);
        }

        // Permite seleccionar un producto de la tabla y abrir directamente el registro de movimiento
        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                // Abrimos el formulario de movimientos (entradas/salidas)
                FrmRegistroSalida frm = new FrmRegistroSalida();
                frm.ShowDialog();
                CargarDatos(""); // Refrescamos al volver
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto de la lista para registrar el movimiento.",
                                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Cierra la sesión del usuario actual y vuelve al formulario de Login
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            regresandoAlMenu = true;

            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                                "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                GUI.Autenticacion.Login.UsuarioAutenticado = null; // Limpiamos la credencial
                this.DialogResult = DialogResult.OK; // Marcamos salida exitosa
                this.Close();
            }
        }

        // Filtra automáticamente cuando se marca/desmarca la opción de stock bajo
        private void checkStockBajo_CheckedChanged(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text);
        }

        // Al intentar cerrar la ventana con la "X", pide confirmación para salir de toda la app
        private void BuscarProductos_FormClosing(object sender, FormClosingEventArgs e)
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
                    e.Cancel = true; // No cierra la ventana
                }
                else
                {
                    // Cierra toda la aplicación
                    Application.ExitThread();
                }
            }
        }
    }
}