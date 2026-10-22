using Hotel.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Repositorios;

public class HabitacionRepositorio
{
    private readonly HotelContext _context;

    public HabitacionRepositorio(HotelContext context)
    {
        _context = context;
    }

    public async Task<List<Habitacion>> ListarActivasAsync(int? idTipo)
    {
        return await _context.Habitacions
            .AsNoTracking()
            .Include(h => h.IdTipoHabitacionNavigation)
            .Where(h => h.Activo && (idTipo == null || h.IdTipoHabitacion == idTipo))
            .OrderBy(h => h.Numero)
            .ToListAsync();
    }

    public async Task<List<TipoHabitacion>> ListarTiposAsync()
    {
        return await _context.TipoHabitacions
            .AsNoTracking()
            .Where(t => t.Activo)
            .OrderBy(t => t.Nombre)
            .ToListAsync();
    }

    public async Task<Habitacion?> ObtenerAsync(int id)
    {
        return await _context.Habitacions
            .AsNoTracking()
            .Include(h => h.IdTipoHabitacionNavigation)
            .FirstOrDefaultAsync(h => h.IdHabitacion == id && h.Activo);
    }

    public async Task<bool> ExisteNumeroAsync(string numero, int idExcluir)
    {
        return await _context.Habitacions
            .AsNoTracking()
            .AnyAsync(h => h.Numero == numero && h.IdHabitacion != idExcluir);
    }

    public async Task CrearAsync(Habitacion habitacion)
    {
        habitacion.Activo = true;
        _context.Habitacions.Add(habitacion);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(Habitacion datos)
    {
        var habitacion = await _context.Habitacions
            .FirstOrDefaultAsync(h => h.IdHabitacion == datos.IdHabitacion && h.Activo);
        if (habitacion == null)
        {
            return false;
        }

        habitacion.Numero = datos.Numero;
        habitacion.Piso = datos.Piso;
        habitacion.IdTipoHabitacion = datos.IdTipoHabitacion;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var habitacion = await _context.Habitacions
            .FirstOrDefaultAsync(h => h.IdHabitacion == id && h.Activo);
        if (habitacion == null)
        {
            return false;
        }

        habitacion.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
