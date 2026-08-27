using System;

namespace lab2.Models
{
    public class Ingreso
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Placa { get; set; }
        public string Turno { get; set; }
        public string Conductor { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public string Transporte { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Peso { get; set; }
    }
}
