using System;
using System.Data;
using System.Threading.Tasks;
using Neptuno.Datos.Modelos;

namespace Neptuno.Datos.Repositorios
{
    public class PedidoRepositorio : RepositorioBase
    {
        public Task<DataTable> ListarAsync()
            => ConsultarAsync("dbo.usp_PedidosListar");

        public Task<DataSet> DetallesPorFechasAsync(DateTime inicio, DateTime fin)
            => ConsultarConjuntoAsync("dbo.usp_DetallePedidosPorFechas", "DetallePedidos",
                Param("@FechaInicio", inicio.Date),
                Param("@FechaFin", fin.Date));

        public Task<int> InsertarAsync(Pedido pedido)
            => EjecutarAsync("dbo.usp_PedidoInsertar",
                Param("@ClienteID", pedido.ClienteID),
                Param("@EmpleadoID", pedido.EmpleadoID),
                Param("@FechaPedido", pedido.FechaPedido),
                Param("@FechaRequerida", pedido.FechaRequerida),
                Param("@FechaEnvio", pedido.FechaEnvio),
                Param("@TransportistaID", pedido.TransportistaID),
                ParamTexto("@Destinatario", pedido.Destinatario),
                ParamTexto("@CiudadDestino", pedido.CiudadDestino),
                ParamTexto("@PaisDestino", pedido.PaisDestino),
                ParamSalida());

        public Task<int> ActualizarAsync(Pedido pedido)
            => EjecutarAsync("dbo.usp_PedidoActualizar",
                Param("@PedidoID", pedido.PedidoID),
                Param("@ClienteID", pedido.ClienteID),
                Param("@EmpleadoID", pedido.EmpleadoID),
                Param("@FechaPedido", pedido.FechaPedido),
                Param("@FechaRequerida", pedido.FechaRequerida),
                Param("@FechaEnvio", pedido.FechaEnvio),
                Param("@TransportistaID", pedido.TransportistaID),
                ParamTexto("@Destinatario", pedido.Destinatario),
                ParamTexto("@CiudadDestino", pedido.CiudadDestino),
                ParamTexto("@PaisDestino", pedido.PaisDestino));

        public Task<int> EliminarAsync(int pedidoId)
            => EjecutarAsync("dbo.usp_PedidoEliminar",
                Param("@PedidoID", pedidoId));
    }
}
