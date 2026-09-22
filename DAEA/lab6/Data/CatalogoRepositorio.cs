using System.Collections.Generic;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.Data
{
    /// <summary>
    /// Tablas de apoyo que alimentan los combos del mantenimiento de pedidos.
    /// Clientes, Empleados y Transportistas no forman parte del enunciado, por
    /// eso no tienen campo Activo ni procedimientos almacenados propios.
    /// </summary>
    public class CatalogoRepositorio
    {
        public List<Cliente> Clientes()
        {
            var lista = new List<Cliente>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand(
                "SELECT ClienteID, Empresa FROM dbo.Clientes ORDER BY Empresa", cn))
            {
                cn.Open();
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Cliente
                        {
                            ClienteID = lector.GetInt32(0),
                            Empresa   = lector.GetString(1)
                        });
                    }
                }
            }

            return lista;
        }

        public List<Empleado> Empleados()
        {
            var lista = new List<Empleado>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand(
                "SELECT EmpleadoID, Nombre + N' ' + Apellidos FROM dbo.Empleados ORDER BY Apellidos", cn))
            {
                cn.Open();
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Empleado
                        {
                            EmpleadoID     = lector.GetInt32(0),
                            NombreCompleto = lector.GetString(1)
                        });
                    }
                }
            }

            return lista;
        }

        public List<Transportista> Transportistas()
        {
            var lista = new List<Transportista>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand(
                "SELECT TransportistaID, CompaniaNombre FROM dbo.Transportistas ORDER BY CompaniaNombre", cn))
            {
                cn.Open();
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Transportista
                        {
                            TransportistaID = lector.GetInt32(0),
                            CompaniaNombre  = lector.GetString(1)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
