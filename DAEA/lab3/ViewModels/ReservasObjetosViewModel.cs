using System;
using System.Collections.Generic;
using System.Windows.Input;
using lab3.Data;
using lab3.Models;

namespace lab3.ViewModels
{
    public class ReservasObjetosViewModel : ViewModelBase
    {
        private readonly ReservaRepositorio _repositorio = new ReservaRepositorio();
        private List<Reserva> _reservas;
        private DateTime? _fecha;

        public ReservasObjetosViewModel()
        {
            Reservas = _repositorio.Listar();
            BuscarCommand = new Comando(Buscar);
        }

        public List<Reserva> Reservas
        {
            get => _reservas;
            set { _reservas = value; Notificar(); }
        }

        public DateTime? Fecha
        {
            get => _fecha;
            set { _fecha = value; Notificar(); }
        }

        public ICommand BuscarCommand { get; }

        private void Buscar()
        {
            Reservas = Fecha.HasValue
                ? _repositorio.BuscarPorFecha(Fecha.Value)
                : _repositorio.Listar();
        }
    }
}
