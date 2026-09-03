using System.Data;
using lab3.Data;

namespace lab3.ViewModels
{
    public class AulasTablaViewModel : ViewModelBase
    {
        private DataView _aulas;

        public AulasTablaViewModel()
        {
            Aulas = new AulaRepositorio().ObtenerTabla().DefaultView;
        }

        public DataView Aulas
        {
            get => _aulas;
            set { _aulas = value; Notificar(); }
        }
    }
}
