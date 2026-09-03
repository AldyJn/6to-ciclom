using System;
using System.Windows.Input;

namespace lab3.ViewModels
{
    public class Comando : ICommand
    {
        private readonly Action _accion;

        public Comando(Action accion)
        {
            _accion = accion;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parametro) => true;

        public void Execute(object parametro) => _accion();
    }
}
