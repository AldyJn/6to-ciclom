using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Neptuno.Datos.Repositorios
{
    public abstract class RepositorioBase
    {
        protected static Task<DataTable> ConsultarAsync(string procedimiento, params SqlParameter[] parametros)
        {
            return Task.Run(() =>
            {
                var tabla = new DataTable();

                using (var cn = new SqlConnection(Conexion.Cadena))
                using (var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure })
                using (var adaptador = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddRange(parametros);
                    adaptador.Fill(tabla);
                }

                return tabla;
            });
        }

        protected static Task<DataTable> ConsultarTextoAsync(string sentencia)
        {
            return Task.Run(() =>
            {
                var tabla = new DataTable();

                using (var cn = new SqlConnection(Conexion.Cadena))
                using (var adaptador = new SqlDataAdapter(sentencia, cn))
                {
                    adaptador.Fill(tabla);
                }

                return tabla;
            });
        }

        protected static Task<DataSet> ConsultarConjuntoAsync(string procedimiento, string nombreTabla, params SqlParameter[] parametros)
        {
            return Task.Run(() =>
            {
                var conjunto = new DataSet("Neptuno");

                using (var cn = new SqlConnection(Conexion.Cadena))
                using (var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure })
                using (var adaptador = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddRange(parametros);
                    adaptador.Fill(conjunto, nombreTabla);
                }

                return conjunto;
            });
        }

        protected static async Task<int> EjecutarAsync(string procedimiento, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddRange(parametros);
                await cn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        protected static SqlParameter Param(string nombre, object valor)
            => new SqlParameter(nombre, valor ?? DBNull.Value);

        protected static SqlParameter ParamTexto(string nombre, string valor)
            => new SqlParameter(nombre, string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim());

        protected static SqlParameter ParamSalida()
            => new SqlParameter("@NuevoID", SqlDbType.Int) { Direction = ParameterDirection.Output };
    }
}
