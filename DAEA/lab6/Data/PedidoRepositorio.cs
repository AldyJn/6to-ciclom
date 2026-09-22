using System;
using System.Collections.Generic;
using System.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.Data
{
    /// <summary>
    /// CRUD de pedidos y reporte de detalles por intervalo de fechas.
    /// Igual que el resto: escrituras con ExecuteNonQuery y baja logica.
    /// </summary>
    public class PedidoRepositorio
    {
        public List<Pedido> Listar()
        {
            var lista = new List<Pedido>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_PedidosListar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Pedido
                        {
                            PedidoID        = Bd.Entero(lector, "PedidoID"),
                            ClienteID       = Bd.EnteroNulo(lector, "ClienteID"),
                            Cliente         = Bd.Texto(lector, "Cliente"),
                            EmpleadoID      = Bd.EnteroNulo(lector, "EmpleadoID"),
                            Empleado        = Bd.Texto(lector, "Empleado"),
                            FechaPedido     = Bd.Fecha(lector, "FechaPedido"),
                            FechaRequerida  = Bd.FechaNula(lector, "FechaRequerida"),
                            FechaEnvio      = Bd.FechaNula(lector, "FechaEnvio"),
                            TransportistaID = Bd.EnteroNulo(lector, "TransportistaID"),
                            Transportista   = Bd.Texto(lector, "Transportista"),
                            Destinatario    = Bd.Texto(lector, "Destinatario"),
                            CiudadDestino   = Bd.Texto(lector, "CiudadDestino"),
                            PaisDestino     = Bd.Texto(lector, "PaisDestino"),
                            Activo          = Bd.Booleano(lector, "Activo")
                        });
                    }
                }
            }

            return lista;
        }

        public int Insertar(Pedido pedido)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_PedidoInsertar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                Cargar(cmd, pedido);

                var nuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(nuevoId);

                cn.Open();
                var filas = cmd.ExecuteNonQuery();      // INSERT

                if (nuevoId.Value != DBNull.Value) pedido.PedidoID = (int)nuevoId.Value;
                return filas;
            }
        }

        public int Actualizar(Pedido pedido)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_PedidoActualizar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", pedido.PedidoID);
                Cargar(cmd, pedido);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE
            }
        }

        /// <summary>
        /// Baja logica: el pedido y su detalle se conservan en la base, pero
        /// con Activo = 0 dejan de aparecer en el listado y en el reporte.
        /// </summary>
        public int Eliminar(int pedidoId)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_PedidoEliminar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", pedidoId);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE Activo = 0
            }
        }

        /// <summary>
        /// Punto 8: detalle de pedidos con inner join a Pedidos, acotado por un
        /// intervalo de fechas y sin los pedidos dados de baja.
        /// </summary>
        public List<DetallePedido> DetallesPorFechas(DateTime desde, DateTime hasta)
        {
            var lista = new List<DetallePedido>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_DetallePedidosPorFechas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", desde.Date);
                cmd.Parameters.AddWithValue("@FechaFin", hasta.Date);

                cn.Open();

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new DetallePedido
                        {
                            PedidoID      = Bd.Entero(lector, "PedidoID"),
                            FechaPedido   = Bd.Fecha(lector, "FechaPedido"),
                            Cliente       = Bd.Texto(lector, "Cliente"),
                            Destinatario  = Bd.Texto(lector, "Destinatario"),
                            CiudadDestino = Bd.Texto(lector, "CiudadDestino"),
                            ProductoID    = Bd.Entero(lector, "ProductoID"),
                            Producto      = Bd.Texto(lector, "Producto"),
                            Categoria     = Bd.Texto(lector, "Categoria"),
                            PrecioUnidad  = Bd.Monto(lector, "PrecioUnidad"),
                            Cantidad      = Bd.Corto(lector, "Cantidad"),
                            Descuento     = Bd.Monto(lector, "Descuento"),
                            Subtotal      = Bd.Monto(lector, "Subtotal")
                        });
                    }
                }
            }

            return lista;
        }

        private static void Cargar(SqlCommand cmd, Pedido pedido)
        {
            cmd.Parameters.AddWithValue("@ClienteID", Bd.Valor(pedido.ClienteID));
            cmd.Parameters.AddWithValue("@EmpleadoID", Bd.Valor(pedido.EmpleadoID));
            cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido.Date);
            cmd.Parameters.AddWithValue("@FechaRequerida", Bd.Valor(pedido.FechaRequerida?.Date));
            cmd.Parameters.AddWithValue("@FechaEnvio", Bd.Valor(pedido.FechaEnvio?.Date));
            cmd.Parameters.AddWithValue("@TransportistaID", Bd.Valor(pedido.TransportistaID));
            cmd.Parameters.AddWithValue("@Destinatario", Bd.Valor(Limpiar(pedido.Destinatario)));
            cmd.Parameters.AddWithValue("@CiudadDestino", Bd.Valor(Limpiar(pedido.CiudadDestino)));
            cmd.Parameters.AddWithValue("@PaisDestino", Bd.Valor(Limpiar(pedido.PaisDestino)));
        }

        private static string Limpiar(string texto)
            => string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
    }
}
