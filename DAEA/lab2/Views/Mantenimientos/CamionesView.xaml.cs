using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Mantenimientos
{
    public partial class CamionesView : UserControl
    {
        public CamionesView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.Camiones;
            cboTransporte.ItemsSource = Datos.Transportistas.Select(t => t.Nombre).ToList();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaca.Text.Trim().Length == 0 || cboTransporte.Text.Trim().Length == 0)
            {
                MessageBox.Show("Complete placa y transporte.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Camiones.Add(new Camion
            {
                Placa = txtPlaca.Text.Trim().ToUpper(),
                Marca = txtMarca.Text.Trim(),
                Transporte = cboTransporte.Text
            });

            txtPlaca.Clear();
            txtMarca.Clear();
        }
    }
}
