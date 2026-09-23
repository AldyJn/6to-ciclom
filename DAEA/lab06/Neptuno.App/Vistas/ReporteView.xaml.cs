using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Neptuno.Datos.Repositorios;

namespace Neptuno.App.Vistas
{
    public partial class ReporteView : UserControl
    {
        private readonly PedidoRepositorio repositorio = new PedidoRepositorio();

        public ReporteView()
        {
            InitializeComponent();
            dpDesde.SelectedDate = new DateTime(DateTime.Today.Year, 1, 1);
            dpHasta.SelectedDate = DateTime.Today;
            Loaded += ReporteView_Loaded;
        }

        private async void ReporteView_Loaded(object sender, RoutedEventArgs e)
        {
            await GenerarAsync();
        }

        private async void Generar_Click(object sender, RoutedEventArgs e)
        {
            await GenerarAsync();
        }

        private async Task GenerarAsync()
        {
            if (dpDesde.SelectedDate == null || dpHasta.SelectedDate == null)
            {
                MessageBox.Show("Indique el intervalo de fechas.");
                return;
            }

            if (dpHasta.SelectedDate < dpDesde.SelectedDate)
            {
                MessageBox.Show("La fecha final no puede ser anterior a la inicial.");
                return;
            }

            try
            {
                var conjunto = await repositorio.DetallesPorFechasAsync(
                    dpDesde.SelectedDate.Value, dpHasta.SelectedDate.Value);

                var tabla = conjunto.Tables["DetallePedidos"];
                grilla.ItemsSource = tabla.DefaultView;

                var total = tabla.Rows.Count == 0
                    ? 0m
                    : tabla.AsEnumerable().Sum(fila => fila.Field<decimal>("Subtotal"));

                lblEstado.Text = tabla.Rows.Count + " detalles - total " + total.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
