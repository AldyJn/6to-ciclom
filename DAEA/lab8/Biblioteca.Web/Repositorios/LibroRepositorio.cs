using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio
{
    private readonly string _cadena;

    public LibroRepositorio(IConfiguration configuracion)
    {
        _cadena = configuracion.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("Falta la cadena de conexión BibliotecaDB.");
    }

    public async Task<IEnumerable<Libro>> ListarAsync()
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.QueryAsync<Libro>("usp_Libros_Listar", commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.QueryAsync<Libro>(
            "usp_Libros_BuscarPorTitulo",
            new { Titulo = titulo },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.QueryFirstOrDefaultAsync<Libro>(
            "usp_Libros_ObtenerPorId",
            new { LibroId = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Libro libro)
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.ExecuteScalarAsync<int>(
            "usp_Libros_Insertar",
            new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarAsync(Libro libro)
    {
        using var conexion = new SqlConnection(_cadena);
        var filas = await conexion.ExecuteScalarAsync<int>(
            "usp_Libros_Actualizar",
            new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
        return filas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conexion = new SqlConnection(_cadena);
        var filas = await conexion.ExecuteScalarAsync<int>(
            "usp_Libros_Eliminar",
            new { LibroId = id },
            commandType: CommandType.StoredProcedure);
        return filas > 0;
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        using var conexion = new SqlConnection(_cadena);
        return await conexion.QueryAsync<Autor>("usp_Autores_ListarActivos", commandType: CommandType.StoredProcedure);
    }
}
