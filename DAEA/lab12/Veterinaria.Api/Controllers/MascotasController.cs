using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Api.Models;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public MascotasController(VeterinariaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas(
        [FromQuery] string? especie, [FromQuery] bool? activa, [FromQuery] decimal? pesoMin, [FromQuery] string? nombre)
    {
        var query = _context.Mascotas.AsNoTracking().Include(m => m.Propietario).AsQueryable();
        if (!string.IsNullOrWhiteSpace(especie))
            query = query.Where(m => m.Especie == especie);
        if (activa.HasValue)
            query = query.Where(m => m.Activa == activa.Value);
        if (pesoMin.HasValue)
            query = query.Where(m => m.PesoKg >= pesoMin.Value);
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(m => m.Nombre.Contains(nombre));
        return Ok(await query.OrderBy(m => m.Nombre).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Mascota>> GetMascota(int id)
    {
        var mascota = await _context.Mascotas.AsNoTracking()
            .Include(m => m.Propietario)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mascota == null) return NotFound();
        return Ok(mascota);
    }

    [HttpPost]
    public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
    {
        if (!await _context.Propietarios.AnyAsync(p => p.Id == mascota.PropietarioId))
            return BadRequest(new { mensaje = "El propietario indicado no existe" });
        if (await _context.Mascotas.AnyAsync(m => m.PropietarioId == mascota.PropietarioId && m.Nombre == mascota.Nombre))
            return Conflict(new { mensaje = "El propietario ya tiene una mascota con ese nombre" });

        mascota.Id = 0;
        mascota.Propietario = null;
        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMascota), new { id = mascota.Id }, mascota);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMascota(int id, Mascota mascota)
    {
        if (id != mascota.Id) return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo" });
        if (!await _context.Mascotas.AnyAsync(m => m.Id == id)) return NotFound();
        if (!await _context.Propietarios.AnyAsync(p => p.Id == mascota.PropietarioId))
            return BadRequest(new { mensaje = "El propietario indicado no existe" });
        if (await _context.Mascotas.AnyAsync(m => m.PropietarioId == mascota.PropietarioId && m.Nombre == mascota.Nombre && m.Id != id))
            return Conflict(new { mensaje = "El propietario ya tiene una mascota con ese nombre" });

        mascota.Propietario = null;
        _context.Entry(mascota).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMascota(int id)
    {
        var mascota = await _context.Mascotas.FindAsync(id);
        if (mascota == null) return NotFound();

        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
