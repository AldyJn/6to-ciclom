using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MascotasController : ControllerBase
{
    private readonly IMascotaService _mascotas;

    public MascotasController(IMascotaService mascotas)
    {
        _mascotas = mascotas;
    }

    [HttpGet]
    public async Task<ActionResult<List<MascotaDto>>> Get(
        [FromQuery] string? nombre,
        [FromQuery] string? especie,
        [FromQuery] bool? activa)
    {
        var lista = await _mascotas.ObtenerTodasAsync(nombre, especie, activa);
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MascotaDto>> GetPorId(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota == null)
            return NotFound();

        return Ok(mascota);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Recepcionista)]
    public async Task<ActionResult<MascotaDto>> Post(MascotaCrearDto dto)
    {
        try
        {
            var creada = await _mascotas.CrearAsync(dto);
            return CreatedAtAction(nameof(GetPorId), new { id = creada.Id }, creada);
        }
        catch (RecursoNoEncontradoException ex)
        {
            return NotFound(new ProblemDetails { Status = 404, Title = "Recurso no encontrado", Detail = ex.Message });
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Veterinario)]
    public async Task<IActionResult> Put(int id, MascotaActualizarDto dto)
    {
        try
        {
            var actualizada = await _mascotas.ActualizarAsync(id, dto);
            if (!actualizada)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Veterinario)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminada = await _mascotas.EliminarAsync(id);
        if (!eliminada)
            return NotFound();

        return NoContent();
    }
}
