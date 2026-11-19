using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class PropietarioRepository : IPropietarioRepository
{
    private readonly VeterinariaContext _context;

    public PropietarioRepository(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<List<Propietario>> ObtenerTodosAsync(string? nombre, string? documento)
    {
        var consulta = _context.Propietarios.Include(p => p.Mascotas).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(p => p.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(documento))
            consulta = consulta.Where(p => p.Documento == documento);

        return await consulta.OrderBy(p => p.Nombre).ToListAsync();
    }

    public async Task<Propietario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Propietarios
            .Include(p => p.Mascotas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExisteDocumentoAsync(string documento, int? excluirId)
    {
        return await _context.Propietarios
            .AnyAsync(p => p.Documento == documento && (excluirId == null || p.Id != excluirId));
    }

    public async Task<int> ContarMascotasAsync(int propietarioId)
    {
        return await _context.Mascotas.CountAsync(m => m.PropietarioId == propietarioId);
    }

    public async Task<Propietario> CrearAsync(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
        return propietario;
    }

    public async Task ActualizarAsync(Propietario propietario)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Propietario propietario)
    {
        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
    }
}
