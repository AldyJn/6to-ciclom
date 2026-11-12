using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class MascotaService : IMascotaService
{
    private const int MaximoMascotasPorPropietario = 5;

    private readonly IMascotaRepository _mascotas;
    private readonly IPropietarioRepository _propietarios;
    private readonly IMapper _mapper;

    public MascotaService(IMascotaRepository mascotas, IPropietarioRepository propietarios, IMapper mapper)
    {
        _mascotas = mascotas;
        _propietarios = propietarios;
        _mapper = mapper;
    }

    public async Task<List<MascotaDto>> ObtenerTodasAsync(string? nombre, string? especie, bool? activa)
    {
        var mascotas = await _mascotas.ObtenerTodasAsync(nombre, especie, activa);
        return _mapper.Map<List<MascotaDto>>(mascotas);
    }

    public async Task<List<MascotaDto>> ObtenerPorPropietarioAsync(int propietarioId)
    {
        var mascotas = await _mascotas.ObtenerPorPropietarioAsync(propietarioId);
        return _mapper.Map<List<MascotaDto>>(mascotas);
    }

    public async Task<MascotaDto?> ObtenerPorIdAsync(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        return mascota == null ? null : _mapper.Map<MascotaDto>(mascota);
    }

    public async Task<MascotaDto> CrearAsync(MascotaCrearDto dto)
    {
        var propietario = await _propietarios.ObtenerPorIdAsync(dto.PropietarioId);
        if (propietario == null)
            throw new RecursoNoEncontradoException($"No existe el propietario con id {dto.PropietarioId}");

        ValidarFechaNacimiento(dto.FechaNacimiento);

        if (await _propietarios.ContarMascotasAsync(dto.PropietarioId) >= MaximoMascotasPorPropietario)
            throw new ReglaNegocioException($"Un propietario no puede tener mas de {MaximoMascotasPorPropietario} mascotas");

        if (!string.IsNullOrWhiteSpace(dto.Microchip) && await _mascotas.ExisteMicrochipAsync(dto.Microchip, null))
            throw new ReglaNegocioException($"El microchip {dto.Microchip} ya esta registrado");

        var mascota = _mapper.Map<Mascota>(dto);
        var creada = await _mascotas.CrearAsync(mascota);
        return _mapper.Map<MascotaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota == null)
            return false;

        ValidarFechaNacimiento(dto.FechaNacimiento);

        if (!string.IsNullOrWhiteSpace(dto.Microchip) && await _mascotas.ExisteMicrochipAsync(dto.Microchip, id))
            throw new ReglaNegocioException($"El microchip {dto.Microchip} ya esta registrado en otra mascota");

        _mapper.Map(dto, mascota);
        await _mascotas.ActualizarAsync(mascota);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota == null)
            return false;

        await _mascotas.EliminarAsync(mascota);
        return true;
    }

    private static void ValidarFechaNacimiento(DateOnly? fecha)
    {
        if (fecha.HasValue && fecha.Value > DateOnly.FromDateTime(DateTime.Today))
            throw new ReglaNegocioException("La fecha de nacimiento no puede ser futura");
    }
}
