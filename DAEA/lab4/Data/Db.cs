using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace lab4.Data
{
    public static class Db
    {
        private const string Cadena =
            @"Server=.\SQLEXPRESS;Database=NeptunoDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlParameter P(string nombre, object valor)
        {
            if (valor is string s && string.IsNullOrWhiteSpace(s)) valor = null;
            return new SqlParameter(nombre, valor ?? DBNull.Value);
        }

        public static DataTable Consultar(string procedimiento, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(Cadena))
            using (var cmd = new SqlCommand(procedimiento, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parametros);
                var tabla = new DataTable();
                da.Fill(tabla);
                return tabla;
            }
        }

        public static int Ejecutar(string procedimiento, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(Cadena))
            using (var cmd = new SqlCommand(procedimiento, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parametros);
                cn.Open();
                var resultado = cmd.ExecuteScalar();
                return resultado == null ? 0 : Convert.ToInt32(resultado);
            }
        }
    }
}
