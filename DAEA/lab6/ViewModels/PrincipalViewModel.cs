using System.Windows.Input;

namespace lab6.ViewModels
{
    public class PrincipalViewModel : ViewModelBase
    {
        private object _vista;

        public PrincipalViewModel()
        {
            ProductosCommand   = new Comando(() => Vista = new ProductosViewModel());
            CategoriasCommand  = new Comando(() => Vista = new CategoriasViewModel());
            ProveedoresCommand = new Comando(() => Vista = new ProveedoresViewModel());
            PedidosCommand     = new Comando(() => Vista = new PedidosViewModel());
            ReporteCommand     = new Comando(() => Vista = new ReportePedidosViewModel());

            Vista = new ProductosViewModel();
        }

        public object Vista
        {
            get => _vista;
            set { _vista = value; Notificar(); }
        }

        public ICommand ProductosCommand { get; }
        public ICommand CategoriasCommand { get; }
        public ICommand ProveedoresCommand { get; }
        public ICommand PedidosCommand { get; }
        public ICommand ReporteCommand { get; }
    }
}
