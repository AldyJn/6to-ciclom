using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios
{
    public class EventoRepositorio
    {
        private readonly EventosContext _context;

        public EventoRepositorio(EventosContext context)
        {
            _context = context;
        }

        public async Task<List<EventoListaVM>> ListarActivosAsync(int? categoriaId)
        {
            return await ProyectarLista(
                    _context.Eventos.AsNoTracking()
                        .Where(e => e.Activo && (categoriaId == null || e.CategoriaId == categoriaId)))
                .OrderBy(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<EventoListaVM?> ObtenerDetalleAsync(int id)
        {
            return await ProyectarLista(
                    _context.Eventos.AsNoTracking().Where(e => e.Id == id && e.Activo))
                .FirstOrDefaultAsync();
        }

        public async Task<Evento?> ObtenerAsync(int id)
        {
            return await _context.Eventos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        }

        public async Task<List<SelectListItem>> CategoriasActivasAsync()
        {
            return await _context.Categorias.AsNoTracking()
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem(c.Nombre, c.Id.ToString()))
                .ToListAsync();
        }

        public async Task CrearAsync(Evento evento)
        {
            evento.Activo = true;
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarAsync(Evento datos)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == datos.Id && e.Activo);
            if (evento == null) return false;

            evento.Titulo = datos.Titulo;
            evento.Fecha = datos.Fecha;
            evento.Lugar = datos.Lugar;
            evento.Aforo = datos.Aforo;
            evento.Precio = datos.Precio;
            evento.CategoriaId = datos.CategoriaId;
            evento.Modalidad = datos.Modalidad;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.Activo);
            if (evento == null) return false;

            evento.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private static IQueryable<EventoListaVM> ProyectarLista(IQueryable<Evento> consulta)
        {
            return consulta.Select(e => new EventoListaVM
            {
                Id = e.Id,
                Titulo = e.Titulo,
                Fecha = e.Fecha,
                Lugar = e.Lugar,
                Aforo = e.Aforo,
                Precio = e.Precio,
                Categoria = e.Categoria!.Nombre,
                Modalidad = e.Modalidad,
                Inscritos = e.Inscripciones.Count(i => i.Estado == "Confirmada")
            });
        }
    }
}
