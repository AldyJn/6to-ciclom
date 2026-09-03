using System;
using System.Collections.Generic;
using System.Windows.Input;
using lab3.Data;
using lab3.Models;

namespace lab3.ViewModels
{
    public class NuevaReservaViewModel : ViewModelBase
    {
        private readonly ReservaRepositorio _repositorio = new ReservaRepositorio();
        private Aula _aula;
        private DateTime? _fecha = DateTime.Today;
        private string _hora = "08:00";
        private string _motivo;
        private string _mensaje;

        public NuevaReservaViewModel()
        {
            Aulas = new AulaRepositorio().Listar();
            Horas = new List<string>();
            for (var h = 7; h <= 21; h++) Horas.Add(h.ToString("00") + ":00");
            GuardarCommand = new Comando(Guardar);
        }

        public List<Aula> Aulas { get; }

        public List<string> Horas { get; }

        public Aula Aula
        {
            get => _aula;
            set { _aula = value; Notificar(); }
        }

        public DateTime? Fecha
        {
            get => _fecha;
            set { _fecha = value; Notificar(); }
        }

        public string Hora
        {
            get => _hora;
            set { _hora = value; Notificar(); }
        }

        public string Motivo
        {
            get => _motivo;
            set { _motivo = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        private void Guardar()
        {
            if (Aula == null || !Fecha.HasValue || string.IsNullOrWhiteSpace(Hora) || string.IsNullOrWhiteSpace(Motivo))
            {
                Mensaje = "Complete todos los campos";
                return;
            }

            var hora = TimeSpan.Parse(Hora);

            if (_repositorio.Existe(Aula.AulaId, Fecha.Value, hora))
            {
                Mensaje = "Ya existe una reserva para esa aula en esa fecha y hora";
                return;
            }

            _repositorio.Insertar(new Reserva
            {
                AulaId = Aula.AulaId,
                UsuarioId = Sesion.Usuario.UsuarioId,
                Fecha = Fecha.Value,
                Hora = hora,
                Motivo = Motivo
            });

            Motivo = string.Empty;
            Mensaje = "Reserva registrada correctamente";
        }

        public ICommand GuardarCommand { get; }
    }
}
