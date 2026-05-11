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
    public partial class ListaRegistroSalida : Form
    {
        // Instanciamos las BLL necesarias
        RegistroSalidaBLL registroBLL = new RegistroSalidaBLL();

        public ListaRegistroSalida()
        {
            InitializeComponent();

            dtpDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpHasta.Value = DateTime.Now;
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvRegistroSalida.Rows.Clear();
            dgvRegistroSalida.Columns.Clear();

            dgvRegistroSalida.Columns.Add("Fecha", "Fecha y Hora");
            dgvRegistroSalida.Columns.Add("Producto", "Producto");
            dgvRegistroSalida.Columns.Add("Tipo", "Tipo");
            dgvRegistroSalida.Columns.Add("Cantidad", "Cantidad");
            dgvRegistroSalida.Columns.Add("Motivo", "Motivo");
            dgvRegistroSalida.Columns.Add("Usuario", "Usuario");

            dgvRegistroSalida.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRegistroSalida.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRegistroSalida.AllowUserToAddRows = false;
            dgvRegistroSalida.ReadOnly = true;

            CargarDatos("");

        }

        private void CargarDatos(string filtroFecha)
        {

            if (dgvRegistroSalida.ColumnCount == 0) return;

            dgvRegistroSalida.Rows.Clear();

            DateTime fechaDesde = dtpDesde.Value.Date;
            DateTime fechaHasta = dtpHasta.Value.Date;

            var movimientos = registroBLL.ObtenerRegistrosSalida();

            var listaFiltrada = movimientos
                .Where(m => m.FechaSalida.Date >= fechaDesde &&
                            m.FechaSalida.Date <= fechaHasta)
                .OrderByDescending(m => m.FechaSalida) 
                .ToList();

            listaFiltrada.ForEach(m =>
            {
                dgvRegistroSalida.Rows.Add(
                    m.FechaSalida.ToString("dd/MM/yyyy HH:mm"),
                    m.Producto?.Nombre ?? "N/A",
                    m.Tipo,
                    m.Cantidad,
                    m.Motivo,
                    m.Usuario?.Nombre ?? "N/A"
                );
            });
        }

        private void btnRegresarMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            CargarDatos("");
        }

        private void dtpHasta_ValueChanged(object sender, EventArgs e)
        {
            CargarDatos("");
        }
    }
}