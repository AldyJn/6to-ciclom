using System;
using System.Collections.Generic;
using System.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.Data
{
    /// <summary>
    /// CRUD de proveedores mas la busqueda por nombre de contacto y ciudad.
    /// Todas las escrituras pasan por ExecuteNonQuery y la baja es logica.
    /// </summary>
    public class ProveedorRepositorio
    {
        public List<Proveedor> Listar()
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProveedoresListar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return Leer(cmd);
            }
        }

        /// <summary>
        /// Punto 7: los dos filtros son opcionales. El procedimiento ignora el
        /// que llegue vacio y siempre restringe el resultado a Activo = 1.
        /// </summary>
        public List<Proveedor> Buscar(string nombreContacto, string ciudad)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProveedoresBuscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreContacto", Bd.Valor(Limpiar(nombreContacto)));
                cmd.Parameters.AddWithValue("@Ciudad", Bd.Valor(Limpiar(ciudad)));

                cn.Open();
                return Leer(cmd);
            }
        }

        public int Insertar(Proveedor proveedor)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProveedorInsertar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                Cargar(cmd, proveedor);

                var nuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(nuevoId);

                cn.Open();
                var filas = cmd.ExecuteNonQuery();      // INSERT

                if (nuevoId.Value != DBNull.Value) proveedor.ProveedorID = (int)nuevoId.Value;
                return filas;
            }
        }

        public int Actualizar(Proveedor proveedor)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProveedorActualizar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                Cargar(cmd, proveedor);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE
            }
        }

        /// <summary>Baja logica: UPDATE dbo.Proveedores SET Activo = 0.</summary>
        public int Eliminar(int proveedorId)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_ProveedorEliminar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE Activo = 0
            }
        }

        private static void Cargar(SqlCommand cmd, Proveedor proveedor)
        {
            cmd.Parameters.AddWithValue("@CompaniaNombre", proveedor.CompaniaNombre);
            cmd.Parameters.AddWithValue("@NombreContacto", Bd.Valor(Limpiar(proveedor.NombreContacto)));
            cmd.Parameters.AddWithValue("@CargoContacto", Bd.Valor(Limpiar(proveedor.CargoContacto)));
            cmd.Parameters.AddWithValue("@Direccion", Bd.Valor(Limpiar(proveedor.Direccion)));
            cmd.Parameters.AddWithValue("@Ciudad", Bd.Valor(Limpiar(proveedor.Ciudad)));
            cmd.Parameters.AddWithValue("@CodigoPostal", Bd.Valor(Limpiar(proveedor.CodigoPostal)));
            cmd.Parameters.AddWithValue("@Pais", Bd.Valor(Limpiar(proveedor.Pais)));
            cmd.Parameters.AddWithValue("@Telefono", Bd.Valor(Limpiar(proveedor.Telefono)));
            cmd.Parameters.AddWithValue("@Fax", Bd.Valor(Limpiar(proveedor.Fax)));
        }

        private static string Limpiar(string texto)
            => string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();

        private static List<Proveedor> Leer(SqlCommand cmd)
        {
            var lista = new List<Proveedor>();

            using (var lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    lista.Add(new Proveedor
                    {
                        ProveedorID    = Bd.Entero(lector, "ProveedorID"),
                        CompaniaNombre = Bd.Texto(lector, "CompaniaNombre"),
                        NombreContacto = Bd.Texto(lector, "NombreContacto"),
                        CargoContacto  = Bd.Texto(lector, "CargoContacto"),
                        Direccion      = Bd.Texto(lector, "Direccion"),
                        Ciudad         = Bd.Texto(lector, "Ciudad"),
                        CodigoPostal   = Bd.Texto(lector, "CodigoPostal"),
                        Pais           = Bd.Texto(lector, "Pais"),
                        Telefono       = Bd.Texto(lector, "Telefono"),
                        Fax            = Bd.Texto(lector, "Fax"),
                        Activo         = Bd.Booleano(lector, "Activo")
                    });
                }
            }

            return lista;
        }
    }
}
