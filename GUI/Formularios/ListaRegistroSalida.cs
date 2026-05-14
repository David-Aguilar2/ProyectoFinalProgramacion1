using BLL;
using EL;
using GUI.Formularios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Formularios
{
    // Formulario que muestra el historial de entradas y salidas de mercancía
    public partial class ListaRegistroSalida : Form
    {
        // Conexión con la lógica de registros
        RegistroSalidaBLL registroBLL = new RegistroSalidaBLL();

        // Variable para saber si el usuario vuelve al menú o cierra el programa
        private bool regresandoAlMenu = false;

        public ListaRegistroSalida()
        {
            InitializeComponent();

            // Configura los calendarios para mostrar por defecto los movimientos del mes actual
            dtpDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpHasta.Value = DateTime.Now;

            ConfigurarGrid(); // Prepara la tabla
        }

        // Configura los encabezados de la tabla de historial
        private void ConfigurarGrid()
        {
            dgvRegistroSalida.Rows.Clear();
            dgvRegistroSalida.Columns.Clear();

            // Definimos qué columnas queremos ver
            dgvRegistroSalida.Columns.Add("Fecha", "Fecha y Hora");
            dgvRegistroSalida.Columns.Add("Producto", "Producto");
            dgvRegistroSalida.Columns.Add("Tipo", "Tipo");
            dgvRegistroSalida.Columns.Add("Cantidad", "Cantidad");
            dgvRegistroSalida.Columns.Add("Motivo", "Motivo");
            dgvRegistroSalida.Columns.Add("Usuario", "Usuario");

            // Ajustes visuales: que no se pueda editar directamente en la tabla y que use todo el espacio
            dgvRegistroSalida.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRegistroSalida.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRegistroSalida.AllowUserToAddRows = false;
            dgvRegistroSalida.ReadOnly = true;

            CargarDatos(""); // Llena la tabla con la información
        }

        // Busca los movimientos en la base de datos y los filtra según el rango de fechas elegido
        private void CargarDatos(string filtroFecha)
        {
            if (dgvRegistroSalida.ColumnCount == 0) return;

            dgvRegistroSalida.Rows.Clear();

            // Captura las fechas seleccionadas en los calendarios
            DateTime fechaDesde = dtpDesde.Value.Date;
            DateTime fechaHasta = dtpHasta.Value.Date;

            // Obtiene todos los registros guardados
            var movimientos = registroBLL.ObtenerRegistrosSalida();

            // Filtra la lista: solo los que estén dentro del rango de fechas y ordenados del más reciente al más antiguo
            var listaFiltrada = movimientos
                .Where(m => m.FechaSalida.Date >= fechaDesde &&
                            m.FechaSalida.Date <= fechaHasta)
                .OrderByDescending(m => m.FechaSalida)
                .ToList();

            // Agrega cada movimiento encontrado a la tabla
            listaFiltrada.ForEach(m =>
            {
                dgvRegistroSalida.Rows.Add(
                    m.FechaSalida.ToString("dd/MM/yyyy HH:mm"), // Formato de fecha legible
                    m.Producto?.Nombre ?? "N/A", // Si el producto fue borrado, muestra N/A
                    m.Tipo,
                    m.Cantidad,
                    m.Motivo,
                    m.Usuario?.Nombre ?? "N/A" // Si el usuario no existe, muestra N/A
                );
            });
        }

        // Botón para cerrar esta ventana y regresar al menú anterior
        private void btnRegresarMenu_Click(object sender, EventArgs e)
        {
            regresandoAlMenu = true;
            this.Close();
        }

        // Actualiza la tabla automáticamente si el usuario cambia la fecha "Desde"
        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            CargarDatos("");
        }

        // Actualiza la tabla automáticamente si el usuario cambia la fecha "Hasta"
        private void dtpHasta_ValueChanged(object sender, EventArgs e)
        {
            CargarDatos("");
        }

        // Controla qué pasa al cerrar la ventana con la "X"
        private void ListaRegistroSalida_FormClosing(object sender, FormClosingEventArgs e)
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
                    e.Cancel = true; // Evita que la ventana se cierre
                }
                else
                {
                    // Si acepta salir, cierra toda la aplicación
                    Application.ExitThread();
                }
            }
        }
    }
}