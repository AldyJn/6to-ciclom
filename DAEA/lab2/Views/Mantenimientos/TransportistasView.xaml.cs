using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Mantenimientos
{
    public partial class TransportistasView : UserControl
    {
        public TransportistasView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.Transportistas;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text.Trim().Length == 0 || txtRuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Complete nombre y RUC.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Transportistas.Add(new Transportista
            {
                Nombre = txtNombre.Text.Trim(),
                Ruc = txtRuc.Text.Trim(),
                Telefono = txtTelefono.Text.Trim()
            });

            txtNombre.Clear();
            txtRuc.Clear();
            txtTelefono.Clear();
        }
    }
}
