using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers
{
    public class EventosController : Controller
    {
        private static readonly string[] Modalidades = { "Presencial", "Virtual" };

        private readonly EventoRepositorio _repo;

        public EventosController(EventoRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(int? categoriaId)
        {
            ViewBag.Categorias = new SelectList(await _repo.CategoriasActivasAsync(), "Value", "Text", categoriaId);
            ViewBag.CategoriaId = categoriaId;
            return View(await _repo.ListarActivosAsync(categoriaId));
        }

        public async Task<IActionResult> Details(int id)
        {
            var evento = await _repo.ObtenerDetalleAsync(id);
            if (evento == null) return NotFound();
            return View(evento);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasAsync(null);
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync(evento.CategoriaId);
                return View(evento);
            }

            await _repo.CrearAsync(evento);
            TempData["Mensaje"] = "Evento registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var evento = await _repo.ObtenerAsync(id);
            if (evento == null) return NotFound();

            await CargarListasAsync(evento.CategoriaId);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(evento.CategoriaId);
                return View(evento);
            }

            if (!await _repo.ActualizarAsync(evento)) return NotFound();

            TempData["Mensaje"] = "Evento actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _repo.ObtenerDetalleAsync(id);
            if (evento == null) return NotFound();
            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _repo.EliminarLogicoAsync(id)) return NotFound();

            TempData["Mensaje"] = "Evento eliminado.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasAsync(int? categoriaId)
        {
            ViewBag.Categorias = new SelectList(await _repo.CategoriasActivasAsync(), "Value", "Text", categoriaId);
            ViewBag.Modalidades = new SelectList(Modalidades);
        }
    }
}
