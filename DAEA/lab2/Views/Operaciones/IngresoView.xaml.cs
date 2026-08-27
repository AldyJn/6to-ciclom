using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Operaciones
{
    public partial class IngresoView : UserControl
    {
        public IngresoView()
        {
            InitializeComponent();
            cboTipoDocumento.ItemsSource = Datos.TiposDocumento;
            cboTurno.ItemsSource = Datos.Turnos;
            cboPlaca.ItemsSource = Datos.Camiones.Select(c => c.Placa).ToList();
            cboConductor.ItemsSource = Datos.Conductores.Select(c => c.Nombre).ToList();
            cboProducto.ItemsSource = Datos.Productos.Select(p => p.Nombre).ToList();
            cboTransporte.ItemsSource = Datos.Transportistas.Select(t => t.Nombre).ToList();
            Limpiar();
        }

        private void Limpiar()
        {
            cboTipoDocumento.SelectedIndex = 0;
            txtNumeroDocumento.Clear();
            cboPlaca.Text = "";
            cboTurno.SelectedIndex = 0;
            cboConductor.Text = "";
            txtCliente.Clear();
            cboProducto.SelectedIndex = 0;
            cboTransporte.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Today;
            txtHora.Text = DateTime.Now.ToString("HH:mm");
            txtPeso.Clear();
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e) => Limpiar();

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumeroDocumento.Text.Trim().Length == 0 || cboPlaca.Text.Trim().Length == 0 ||
                cboConductor.Text.Trim().Length == 0 || txtCliente.Text.Trim().Length == 0)
            {
                MessageBox.Show("Complete los campos obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPeso.Text.Trim(), out decimal peso) || peso <= 0)
            {
                MessageBox.Show("Ingrese un peso válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpFecha.SelectedDate == null || !TimeSpan.TryParse(txtHora.Text.Trim(), out TimeSpan hora))
            {
                MessageBox.Show("Ingrese una fecha y hora válidas.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Ingresos.Add(new Ingreso
            {
                TipoDocumento = cboTipoDocumento.Text,
                NumeroDocumento = txtNumeroDocumento.Text.Trim(),
                Placa = cboPlaca.Text.Trim().ToUpper(),
                Turno = cboTurno.Text,
                Conductor = cboConductor.Text.Trim(),
                Cliente = txtCliente.Text.Trim(),
                Producto = cboProducto.Text,
                Transporte = cboTransporte.Text,
                Fecha = dpFecha.SelectedDate.Value.Date + hora,
                Peso = peso
            });

            MessageBox.Show("Ingreso registrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            Limpiar();
        }
    }
}
