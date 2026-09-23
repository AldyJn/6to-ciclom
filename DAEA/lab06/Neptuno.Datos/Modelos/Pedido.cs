using System;

namespace Neptuno.Datos.Modelos
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int? ClienteID { get; set; }
        public int? EmpleadoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaRequerida { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? TransportistaID { get; set; }
        public string Destinatario { get; set; }
        public string CiudadDestino { get; set; }
        public string PaisDestino { get; set; }
    }
}
