using System.Data;
using lab3.Data;

namespace lab3.ViewModels
{
    public class ReservasTablaViewModel : ViewModelBase
    {
        private DataView _reservas;

        public ReservasTablaViewModel()
        {
            Reservas = new ReservaRepositorio().ObtenerTabla().DefaultView;
        }

        public DataView Reservas
        {
            get => _reservas;
            set { _reservas = value; Notificar(); }
        }
    }
}
