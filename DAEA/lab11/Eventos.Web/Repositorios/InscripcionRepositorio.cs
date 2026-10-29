using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios
{
    public class InscripcionRepositorio
    {
        private readonly EventosContext _context;

        public InscripcionRepositorio(EventosContext context)
        {
            _context = context;
        }

        public async Task<List<Inscripcion>> ListarAsync()
        {
            return await _context.Inscripciones.AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Asistente)
                .OrderByDescending(i => i.FechaInscripcion)
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> EventosActivosAsync()
        {
            var eventos = await _context.Eventos.AsNoTracking()
                .Where(e => e.Activo)
                .OrderBy(e => e.Fecha)
                .Select(e => new { e.Id, e.Titulo, e.Fecha })
                .ToListAsync();

            return eventos
                .Select(e => new SelectListItem($"{e.Titulo} ({e.Fecha:dd/MM/yyyy})", e.Id.ToString()))
                .ToList();
        }

        public async Task<List<SelectListItem>> AsistentesActivosAsync()
        {
            return await _context.Asistentes.AsNoTracking()
                .Where(a => a.Activo)
                .OrderBy(a => a.Nombres)
                .Select(a => new SelectListItem(a.Nombres + " - " + a.Dni, a.Id.ToString()))
                .ToListAsync();
        }

        public async Task<Evento?> ObtenerEventoAsync(int eventoId)
        {
            return await _context.Eventos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventoId && e.Activo);
        }

        public async Task<int> ContarConfirmadasAsync(int eventoId)
        {
            return await _context.Inscripciones.AsNoTracking()
                .CountAsync(i => i.EventoId == eventoId && i.Estado == "Confirmada");
        }

        public async Task<bool> YaInscritoAsync(int eventoId, int asistenteId)
        {
            return await _context.Inscripciones.AsNoTracking()
                .AnyAsync(i => i.EventoId == eventoId && i.AsistenteId == asistenteId && i.Estado == "Confirmada");
        }

        public async Task CrearAsync(Inscripcion inscripcion)
        {
            inscripcion.FechaInscripcion = DateTime.Now;
            inscripcion.Estado = "Confirmada";
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.Id == id && i.Estado == "Confirmada");
            if (inscripcion == null) return false;

            inscripcion.Estado = "Cancelada";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
