using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IPropietarioRepository
{
    Task<List<Propietario>> ObtenerTodosAsync(string? nombre, string? documento);

    Task<Propietario?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteDocumentoAsync(string documento, int? excluirId);

    Task<int> ContarMascotasAsync(int propietarioId);

    Task<Propietario> CrearAsync(Propietario propietario);

    Task ActualizarAsync(Propietario propietario);

    Task EliminarAsync(Propietario propietario);
}
