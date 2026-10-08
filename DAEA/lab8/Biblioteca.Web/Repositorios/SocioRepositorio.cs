using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio
{
    private readonly string _cadena;

    public SocioRepositorio(IConfiguration configuracion)
    {
        _cadena = configuracion.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("Falta la cadena de conexión BibliotecaDB.");
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.QueryAsync<Socio>("usp_Socios_Listar", commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Socio socio)
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.ExecuteScalarAsync<int>(
            "usp_Socios_Insertar",
            new { socio.DNI, socio.Nombre, socio.Email },
            commandType: CommandType.StoredProcedure);
    }
}
