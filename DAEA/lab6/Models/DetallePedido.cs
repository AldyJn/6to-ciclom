using System;

namespace lab6.Models
{
    public class DetallePedido
    {
        public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Cliente { get; set; }
        public string Destinatario { get; set; }
        public string CiudadDestino { get; set; }
        public int ProductoID { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }
}
