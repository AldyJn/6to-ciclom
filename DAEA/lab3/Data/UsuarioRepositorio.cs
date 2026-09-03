using lab3.Models;
using Microsoft.Data.SqlClient;

namespace lab3.Data
{
    public class UsuarioRepositorio
    {
        public Usuario Validar(string username, string password)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT UsuarioId, Username, NombreCompleto FROM Usuarios WHERE Username = @u AND Password = @p", cn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                using (var lector = cmd.ExecuteReader())
                {
                    if (!lector.Read()) return null;

                    return new Usuario
                    {
                        UsuarioId = lector.GetInt32(0),
                        Username = lector.GetString(1),
                        NombreCompleto = lector.GetString(2)
                    };
                }
            }
        }
    }
}
