using System;
using System.Collections.Generic;
using System.Data;
using lab3.Models;
using Microsoft.Data.SqlClient;

namespace lab3.Data
{
    public class ReservaRepositorio
    {
        private const string Consulta =
            "SELECT r.ReservaId, r.AulaId, a.Nombre AS Aula, r.UsuarioId, u.NombreCompleto AS Usuario, " +
            "r.Fecha, r.Hora, r.Motivo " +
            "FROM Reservas r " +
            "INNER JOIN Aulas a ON a.AulaId = r.AulaId " +
            "INNER JOIN Usuarios u ON u.UsuarioId = r.UsuarioId";

        public DataTable ObtenerTabla()
        {
            var tabla = new DataTable();

            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                var adaptador = new SqlDataAdapter(Consulta + " ORDER BY r.ReservaId", cn);
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        public List<Reserva> Listar()
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(Consulta + " ORDER BY r.ReservaId", cn);
                return Leer(cmd);
            }
        }

        public List<Reserva> BuscarPorFecha(DateTime fecha)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(Consulta + " WHERE r.Fecha = @f ORDER BY r.Hora", cn);
                cmd.Parameters.AddWithValue("@f", fecha.Date);
                return Leer(cmd);
            }
        }

        public bool Existe(int aulaId, DateTime fecha, TimeSpan hora)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservas WHERE AulaId = @a AND Fecha = @f AND Hora = @h", cn);
                cmd.Parameters.AddWithValue("@a", aulaId);
                cmd.Parameters.AddWithValue("@f", fecha.Date);
                cmd.Parameters.AddWithValue("@h", hora);

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Insertar(Reserva reserva)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo) VALUES (@a, @u, @f, @h, @m)", cn);
                cmd.Parameters.AddWithValue("@a", reserva.AulaId);
                cmd.Parameters.AddWithValue("@u", reserva.UsuarioId);
                cmd.Parameters.AddWithValue("@f", reserva.Fecha.Date);
                cmd.Parameters.AddWithValue("@h", reserva.Hora);
                cmd.Parameters.AddWithValue("@m", reserva.Motivo);
                cmd.ExecuteNonQuery();
            }
        }

        private List<Reserva> Leer(SqlCommand cmd)
        {
            var lista = new List<Reserva>();

            using (var lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    lista.Add(new Reserva
                    {
                        ReservaId = lector.GetInt32(0),
                        AulaId = lector.GetInt32(1),
                        Aula = lector.GetString(2),
                        UsuarioId = lector.GetInt32(3),
                        Usuario = lector.GetString(4),
                        Fecha = lector.GetDateTime(5),
                        Hora = lector.GetTimeSpan(6),
                        Motivo = lector.GetString(7)
                    });
                }
            }

            return lista;
        }
    }
}
