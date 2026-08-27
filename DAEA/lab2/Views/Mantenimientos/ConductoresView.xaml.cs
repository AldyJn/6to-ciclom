using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;
using lab2.Services;

namespace lab2.Views.Mantenimientos
{
    public partial class ConductoresView : UserControl
    {
        public ConductoresView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.Conductores;
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ConductorView());
    }
}
