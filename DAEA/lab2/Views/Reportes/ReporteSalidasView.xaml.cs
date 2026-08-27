using System.Windows.Controls;
using lab2.Data;

namespace lab2.Views.Reportes
{
    public partial class ReporteSalidasView : UserControl
    {
        public ReporteSalidasView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.Salidas;
        }
    }
}
