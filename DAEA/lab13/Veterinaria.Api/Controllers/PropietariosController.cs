using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropietariosController : ControllerBase
{
    private readonly IPropietarioService _propietarios;
    private readonly IMascotaService _mascotas;

    public PropietariosController(IPropietarioService propietarios, IMascotaService mascotas)
    {
        _propietarios = propietarios;
        _mascotas = mascotas;
    }

    [HttpGet]
    public async Task<ActionResult<List<PropietarioDto>>> Get([FromQuery] string? nombre, [FromQuery] string? documento)
    {
        var lista = await _propietarios.ObtenerTodosAsync(nombre, documento);
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropietarioDto>> GetPorId(int id)
    {
        var propietario = await _propietarios.ObtenerPorIdAsync(id);
        if (propietario == null)
            return NotFound();

        return Ok(propietario);
    }

    [HttpGet("{id:int}/mascotas")]
    public async Task<ActionResult<List<MascotaDto>>> GetMascotas(int id)
    {
        var propietario = await _propietarios.ObtenerPorIdAsync(id);
        if (propietario == null)
            return NotFound();

        var lista = await _mascotas.ObtenerPorPropietarioAsync(id);
        return Ok(lista);
    }

    [HttpPost]
    public async Task<ActionResult<PropietarioDto>> Post(PropietarioCrearDto dto)
    {
        try
        {
            var creado = await _propietarios.CrearAsync(dto);
            return CreatedAtAction(nameof(GetPorId), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, PropietarioActualizarDto dto)
    {
        try
        {
            var actualizado = await _propietarios.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminado = await _propietarios.EliminarAsync(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }
}
