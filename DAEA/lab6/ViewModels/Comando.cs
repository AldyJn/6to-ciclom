using System;
using System.Windows.Input;

namespace lab6.ViewModels
{
    public class Comando : ICommand
    {
        private readonly Action _accion;
        private readonly Func<bool> _puedeEjecutar;

        public Comando(Action accion, Func<bool> puedeEjecutar = null)
        {
            _accion = accion;
            _puedeEjecutar = puedeEjecutar;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parametro) => _puedeEjecutar == null || _puedeEjecutar();

        public void Execute(object parametro) => _accion();
    }
}
