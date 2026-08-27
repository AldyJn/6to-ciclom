using System.Windows;
using lab2.Services;
using lab2.Views.Mantenimientos;
using lab2.Views.Operaciones;
using lab2.Views.Reportes;

namespace lab2.Views
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();
            Navegacion.Ir = vista => host.Content = vista;
            navIngresos.IsChecked = true;
            Navegacion.Ir(new IngresoView());
        }

        private void Ingresos_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new IngresoView());

        private void Salidas_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new SalidaView());

        private void Conductores_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ConductoresView());

        private void Transportistas_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new TransportistasView());

        private void Camiones_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new CamionesView());

        private void Productos_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ProductosView());

        private void ReporteCargas_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ReporteCargasView());

        private void ReporteIngresos_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ReporteIngresosView());

        private void ReporteSalidas_Click(object sender, RoutedEventArgs e) => Navegacion.Ir(new ReporteSalidasView());

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            Application.Current.MainWindow = login;
            login.Show();
            Close();
        }
    }
}
