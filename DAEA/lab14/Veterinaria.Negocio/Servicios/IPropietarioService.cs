using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IPropietarioService
{
    Task<List<PropietarioDto>> ObtenerTodosAsync(string? nombre, string? documento);

    Task<PropietarioDto?> ObtenerPorIdAsync(int id);

    Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto);

    Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}
