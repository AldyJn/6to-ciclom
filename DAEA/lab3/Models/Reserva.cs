using System;

namespace lab3.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }
        public int AulaId { get; set; }
        public string Aula { get; set; }
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Motivo { get; set; }
    }
}
