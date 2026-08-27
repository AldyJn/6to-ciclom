using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Operaciones
{
    public partial class SalidaView : UserControl
    {
        public SalidaView()
        {
            InitializeComponent();
            cboTurno.ItemsSource = Datos.Turnos;
            cboPlaca.ItemsSource = Datos.Camiones.Select(c => c.Placa).ToList();
            cboConductor.ItemsSource = Datos.Conductores.Select(c => c.Nombre).ToList();
            cboProducto.ItemsSource = Datos.Productos.Select(p => p.Nombre).ToList();
            cboTurno.SelectedIndex = 0;
            cboProducto.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Today;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumeroDocumento.Text.Trim().Length == 0 || cboPlaca.Text.Trim().Length == 0 || cboConductor.Text.Trim().Length == 0)
            {
                MessageBox.Show("Complete los campos obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPeso.Text.Trim(), out decimal peso) || peso <= 0)
            {
                MessageBox.Show("Ingrese un peso válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Salidas.Add(new Salida
            {
                NumeroDocumento = txtNumeroDocumento.Text.Trim(),
                Placa = cboPlaca.Text.Trim().ToUpper(),
                Turno = cboTurno.Text,
                Conductor = cboConductor.Text.Trim(),
                Producto = cboProducto.Text,
                Fecha = (dpFecha.SelectedDate ?? DateTime.Today).Date + DateTime.Now.TimeOfDay,
                Peso = peso
            });

            MessageBox.Show("Salida registrada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            txtNumeroDocumento.Clear();
            cboPlaca.Text = "";
            cboConductor.Text = "";
            txtPeso.Clear();
        }
    }
}
