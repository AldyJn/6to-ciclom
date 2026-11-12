using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class MascotaRepository : IMascotaRepository
{
    private readonly VeterinariaContext _context;

    public MascotaRepository(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<List<Mascota>> ObtenerTodasAsync(string? nombre, string? especie, bool? activa)
    {
        var consulta = _context.Mascotas.Include(m => m.Propietario).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(m => m.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(especie))
            consulta = consulta.Where(m => m.Especie == especie);

        if (activa.HasValue)
            consulta = consulta.Where(m => m.Activa == activa.Value);

        return await consulta.OrderBy(m => m.Nombre).ToListAsync();
    }

    public async Task<List<Mascota>> ObtenerPorPropietarioAsync(int propietarioId)
    {
        return await _context.Mascotas
            .Include(m => m.Propietario)
            .Where(m => m.PropietarioId == propietarioId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorIdAsync(int id)
    {
        return await _context.Mascotas
            .Include(m => m.Propietario)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId)
    {
        return await _context.Mascotas
            .AnyAsync(m => m.Microchip == microchip && (excluirId == null || m.Id != excluirId));
    }

    public async Task<Mascota> CrearAsync(Mascota mascota)
    {
        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();
        await _context.Entry(mascota).Reference(m => m.Propietario).LoadAsync();
        return mascota;
    }

    public async Task ActualizarAsync(Mascota mascota)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Mascota mascota)
    {
        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
    }
}
