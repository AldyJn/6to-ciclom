using System.Configuration;

namespace Neptuno.Datos
{
    public static class Conexion
    {
        public static string Cadena =>
            ConfigurationManager.ConnectionStrings["NeptunoDB"].ConnectionString;
    }
}
