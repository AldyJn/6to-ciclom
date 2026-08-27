using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;
using lab2.Services;

namespace lab2.Views.Mantenimientos
{
    public partial class ConductorView : UserControl
    {
        public ConductorView()
        {
            InitializeComponent();
            cboTransporte.ItemsSource = Datos.Transportistas.Select(t => t.Nombre).ToList();
        }

        private void Volver_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ConductoresView());

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text.Trim().Length == 0 || txtLicencia.Text.Trim().Length == 0 || cboTransporte.Text.Trim().Length == 0)
            {
                MessageBox.Show("Complete todos los campos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Conductores.Add(new Conductor
            {
                Nombre = txtNombre.Text.Trim(),
                Licencia = txtLicencia.Text.Trim(),
                Transporte = cboTransporte.Text.Trim()
            });

            Navegacion.Ir(new ConductoresView());
        }
    }
}
