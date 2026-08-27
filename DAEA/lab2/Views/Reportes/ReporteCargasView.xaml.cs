using System.Windows.Controls;
using lab2.Data;

namespace lab2.Views.Reportes
{
    public partial class ReporteCargasView : UserControl
    {
        public ReporteCargasView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.ObtenerCargas();
        }
    }
}
