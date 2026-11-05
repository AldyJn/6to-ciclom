using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Api.Models;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropietariosController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public PropietariosController(VeterinariaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios([FromQuery] string? nombre, [FromQuery] string? dni)
    {
        var query = _context.Propietarios.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre.Contains(nombre));
        if (!string.IsNullOrWhiteSpace(dni))
            query = query.Where(p => p.Dni == dni);
        return Ok(await query.OrderBy(p => p.Nombre).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Propietario>> GetPropietario(int id)
    {
        var propietario = await _context.Propietarios.AsNoTracking()
            .Include(p => p.Mascotas)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (propietario == null) return NotFound();
        return Ok(propietario);
    }

    [HttpGet("{id}/mascotas")]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotasDePropietario(int id)
    {
        if (!await _context.Propietarios.AnyAsync(p => p.Id == id)) return NotFound();
        var mascotas = await _context.Mascotas.AsNoTracking()
            .Where(m => m.PropietarioId == id)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
        return Ok(mascotas);
    }

    [HttpPost]
    public async Task<ActionResult<Propietario>> PostPropietario(Propietario propietario)
    {
        if (await _context.Propietarios.AnyAsync(p => p.Dni == propietario.Dni))
            return Conflict(new { mensaje = "Ya existe un propietario con ese DNI" });

        propietario.Id = 0;
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPropietario), new { id = propietario.Id }, propietario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPropietario(int id, Propietario propietario)
    {
        if (id != propietario.Id) return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo" });
        if (!await _context.Propietarios.AnyAsync(p => p.Id == id)) return NotFound();
        if (await _context.Propietarios.AnyAsync(p => p.Dni == propietario.Dni && p.Id != id))
            return Conflict(new { mensaje = "Ya existe otro propietario con ese DNI" });

        _context.Entry(propietario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePropietario(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario == null) return NotFound();
        if (await _context.Mascotas.AnyAsync(m => m.PropietarioId == id))
            return Conflict(new { mensaje = "No se puede eliminar un propietario que tiene mascotas registradas" });

        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
