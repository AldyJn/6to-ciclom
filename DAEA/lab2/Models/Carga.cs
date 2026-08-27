using System;

namespace lab2.Models
{
    public class Carga
    {
        public string Placa { get; set; }
        public string Conductor { get; set; }
        public string Producto { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PesoIngreso { get; set; }
        public decimal PesoSalida { get; set; }
        public decimal PesoNeto { get; set; }
    }
}
