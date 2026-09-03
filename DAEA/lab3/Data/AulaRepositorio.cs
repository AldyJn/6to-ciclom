using System.Collections.Generic;
using System.Data;
using lab3.Models;
using Microsoft.Data.SqlClient;

namespace lab3.Data
{
    public class AulaRepositorio
    {
        public DataTable ObtenerTabla()
        {
            var tabla = new DataTable();

            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                var adaptador = new SqlDataAdapter("SELECT AulaId, Nombre, Capacidad FROM Aulas ORDER BY AulaId", cn);
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        public List<Aula> Listar()
        {
            var lista = new List<Aula>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand("SELECT AulaId, Nombre, Capacidad FROM Aulas ORDER BY AulaId", cn);

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Aula
                        {
                            AulaId = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Capacidad = lector.GetInt32(2)
                        });
                    }
                }
            }

            return lista;
        }

        public List<Aula> BuscarPorNombre(string nombre)
        {
            var lista = new List<Aula>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT AulaId, Nombre, Capacidad FROM Aulas WHERE Nombre LIKE @n ORDER BY AulaId", cn);
                cmd.Parameters.AddWithValue("@n", "%" + nombre + "%");

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Aula
                        {
                            AulaId = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Capacidad = lector.GetInt32(2)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
