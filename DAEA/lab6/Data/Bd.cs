using System;
using System.Data;

namespace lab6.Data
{
    /// <summary>
    /// Ayudas para convertir entre los valores de SQL Server y los tipos de C#.
    /// Evita repetir la comprobacion de DBNull en cada repositorio.
    /// </summary>
    internal static class Bd
    {
        /// <summary>Convierte null de C# en DBNull para enviarlo como parametro.</summary>
        public static object Valor(object valor) => valor ?? (object)DBNull.Value;

        public static string Texto(IDataRecord registro, string columna)
        {
            var valor = registro[columna];
            return valor == DBNull.Value ? null : Convert.ToString(valor);
        }

        public static int Entero(IDataRecord registro, string columna)
            => Convert.ToInt32(registro[columna]);

        public static int? EnteroNulo(IDataRecord registro, string columna)
        {
            var valor = registro[columna];
            return valor == DBNull.Value ? (int?)null : Convert.ToInt32(valor);
        }

        public static short Corto(IDataRecord registro, string columna)
            => Convert.ToInt16(registro[columna]);

        public static decimal Monto(IDataRecord registro, string columna)
            => Convert.ToDecimal(registro[columna]);

        public static bool Booleano(IDataRecord registro, string columna)
            => Convert.ToBoolean(registro[columna]);

        public static DateTime Fecha(IDataRecord registro, string columna)
            => Convert.ToDateTime(registro[columna]);

        public static DateTime? FechaNula(IDataRecord registro, string columna)
        {
            var valor = registro[columna];
            return valor == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(valor);
        }
    }
}
