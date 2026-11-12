using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IMascotaRepository
{
    Task<List<Mascota>> ObtenerTodasAsync(string? nombre, string? especie, bool? activa);

    Task<List<Mascota>> ObtenerPorPropietarioAsync(int propietarioId);

    Task<Mascota?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId);

    Task<Mascota> CrearAsync(Mascota mascota);

    Task ActualizarAsync(Mascota mascota);

    Task EliminarAsync(Mascota mascota);
}
