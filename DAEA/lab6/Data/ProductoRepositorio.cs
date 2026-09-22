using System;
using System.Collections.Generic;
using System.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.Data
{
    /// <summary>
    /// CRUD de productos. El listado solo trae los registros con Activo = 1
    /// y Eliminar ejecuta la baja logica, nunca un DELETE fisico.
    /// </summary>
    public class ProductoRepositorio
    {
        public List<Producto> Listar()
        {
            var lista = new List<Producto>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProductosListar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Producto
                        {
                            ProductoID           = Bd.Entero(lector, "ProductoID"),
                            NombreProducto       = Bd.Texto(lector, "NombreProducto"),
                            ProveedorID          = Bd.EnteroNulo(lector, "ProveedorID"),
                            Proveedor            = Bd.Texto(lector, "Proveedor"),
                            CategoriaID          = Bd.EnteroNulo(lector, "CategoriaID"),
                            Categoria            = Bd.Texto(lector, "Categoria"),
                            CantidadPorUnidad    = Bd.Texto(lector, "CantidadPorUnidad"),
                            PrecioUnidad         = Bd.Monto(lector, "PrecioUnidad"),
                            UnidadesEnExistencia = Bd.Corto(lector, "UnidadesEnExistencia"),
                            UnidadesEnPedido     = Bd.Corto(lector, "UnidadesEnPedido"),
                            NivelDeReorden       = Bd.Corto(lector, "NivelDeReorden"),
                            Descontinuado        = Bd.Booleano(lector, "Descontinuado"),
                            Activo               = Bd.Booleano(lector, "Activo")
                        });
                    }
                }
            }

            return lista;
        }

        public int Insertar(Producto producto)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProductoInsertar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                Cargar(cmd, producto);

                var nuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(nuevoId);

                cn.Open();
                var filas = cmd.ExecuteNonQuery();      // INSERT

                if (nuevoId.Value != DBNull.Value) producto.ProductoID = (int)nuevoId.Value;
                return filas;
            }
        }

        public int Actualizar(Producto producto)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProductoActualizar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", producto.ProductoID);
                Cargar(cmd, producto);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE
            }
        }

        /// <summary>
        /// Baja logica: el procedimiento hace UPDATE ... SET Activo = 0, con lo
        /// que el historial de DetallePedidos sigue apuntando a una fila valida.
        /// </summary>
        public int Eliminar(int productoId)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProductoEliminar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", productoId);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE Activo = 0
            }
        }

        private static void Cargar(SqlCommand cmd, Producto producto)
        {
            cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
            cmd.Parameters.AddWithValue("@ProveedorID", Bd.Valor(producto.ProveedorID));
            cmd.Parameters.AddWithValue("@CategoriaID", Bd.Valor(producto.CategoriaID));
            cmd.Parameters.AddWithValue("@CantidadPorUnidad",
                Bd.Valor(string.IsNullOrWhiteSpace(producto.CantidadPorUnidad)
                    ? null : producto.CantidadPorUnidad.Trim()));
            cmd.Parameters.AddWithValue("@PrecioUnidad", producto.PrecioUnidad);
            cmd.Parameters.AddWithValue("@UnidadesEnExistencia", producto.UnidadesEnExistencia);
            cmd.Parameters.AddWithValue("@UnidadesEnPedido", producto.UnidadesEnPedido);
            cmd.Parameters.AddWithValue("@NivelDeReorden", producto.NivelDeReorden);
            cmd.Parameters.AddWithValue("@Descontinuado", producto.Descontinuado);
        }
    }
}
