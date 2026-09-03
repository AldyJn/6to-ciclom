using System.Windows.Input;
using lab3.Data;

namespace lab3.ViewModels
{
    public class PrincipalViewModel : ViewModelBase
    {
        private object _vista;

        public PrincipalViewModel()
        {
            AulasTablaCommand = new Comando(() => Vista = new AulasTablaViewModel());
            AulasObjetosCommand = new Comando(() => Vista = new AulasObjetosViewModel());
            ReservasTablaCommand = new Comando(() => Vista = new ReservasTablaViewModel());
            ReservasObjetosCommand = new Comando(() => Vista = new ReservasObjetosViewModel());
            NuevaReservaCommand = new Comando(() => Vista = new NuevaReservaViewModel());
            Vista = new AulasTablaViewModel();
        }

        public string Usuario => Sesion.Usuario?.NombreCompleto;

        public object Vista
        {
            get => _vista;
            set { _vista = value; Notificar(); }
        }

        public ICommand AulasTablaCommand { get; }
        public ICommand AulasObjetosCommand { get; }
        public ICommand ReservasTablaCommand { get; }
        public ICommand ReservasObjetosCommand { get; }
        public ICommand NuevaReservaCommand { get; }
    }
}
