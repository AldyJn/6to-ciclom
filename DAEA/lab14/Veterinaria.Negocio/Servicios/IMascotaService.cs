using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IMascotaService
{
    Task<List<MascotaDto>> ObtenerTodasAsync(string? nombre, string? especie, bool? activa);

    Task<List<MascotaDto>> ObtenerPorPropietarioAsync(int propietarioId);

    Task<MascotaDto?> ObtenerPorIdAsync(int id);

    Task<MascotaDto> CrearAsync(MascotaCrearDto dto);

    Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}
