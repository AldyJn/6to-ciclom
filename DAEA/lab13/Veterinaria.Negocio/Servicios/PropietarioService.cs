using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class PropietarioService : IPropietarioService
{
    private readonly IPropietarioRepository _repositorio;
    private readonly IMapper _mapper;

    public PropietarioService(IPropietarioRepository repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<PropietarioDto>> ObtenerTodosAsync(string? nombre, string? documento)
    {
        var propietarios = await _repositorio.ObtenerTodosAsync(nombre, documento);
        return _mapper.Map<List<PropietarioDto>>(propietarios);
    }

    public async Task<PropietarioDto?> ObtenerPorIdAsync(int id)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        return propietario == null ? null : _mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto)
    {
        if (await _repositorio.ExisteDocumentoAsync(dto.Documento, null))
            throw new ReglaNegocioException($"Ya existe un propietario con el documento {dto.Documento}");

        var propietario = _mapper.Map<Propietario>(dto);
        var creado = await _repositorio.CrearAsync(propietario);
        return _mapper.Map<PropietarioDto>(creado);
    }

    public async Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        if (propietario == null)
            return false;

        if (await _repositorio.ExisteDocumentoAsync(dto.Documento, id))
            throw new ReglaNegocioException($"Ya existe otro propietario con el documento {dto.Documento}");

        _mapper.Map(dto, propietario);
        await _repositorio.ActualizarAsync(propietario);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        if (propietario == null)
            return false;

        if (await _repositorio.ContarMascotasAsync(id) > 0)
            throw new ReglaNegocioException("No se puede eliminar un propietario que tiene mascotas registradas");

        await _repositorio.EliminarAsync(propietario);
        return true;
    }
}
