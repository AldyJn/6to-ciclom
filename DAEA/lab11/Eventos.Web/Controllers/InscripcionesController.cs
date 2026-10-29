using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly InscripcionRepositorio _repo;

        public InscripcionesController(InscripcionRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.ListarAsync());
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasAsync(null, null);
            return View(new Inscripcion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                var evento = await _repo.ObtenerEventoAsync(inscripcion.EventoId);
                if (evento == null)
                {
                    ModelState.AddModelError(nameof(Inscripcion.EventoId), "El evento seleccionado no existe.");
                }
                else
                {
                    if (evento.Fecha < DateTime.Now)
                        ModelState.AddModelError(nameof(Inscripcion.EventoId), "El evento ya pasó.");

                    if (await _repo.YaInscritoAsync(inscripcion.EventoId, inscripcion.AsistenteId))
                        ModelState.AddModelError(nameof(Inscripcion.AsistenteId), "El asistente ya está inscrito en este evento.");

                    if (await _repo.ContarConfirmadasAsync(inscripcion.EventoId) >= evento.Aforo)
                        ModelState.AddModelError(nameof(Inscripcion.EventoId), "El aforo del evento está completo.");
                }
            }

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(inscripcion.EventoId, inscripcion.AsistenteId);
                return View(inscripcion);
            }

            await _repo.CrearAsync(inscripcion);
            TempData["Mensaje"] = "Inscripción registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            TempData["Mensaje"] = await _repo.CancelarAsync(id)
                ? "Inscripción cancelada."
                : "No se pudo cancelar la inscripción.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasAsync(int? eventoId, int? asistenteId)
        {
            ViewBag.Eventos = new SelectList(await _repo.EventosActivosAsync(), "Value", "Text", eventoId);
            ViewBag.Asistentes = new SelectList(await _repo.AsistentesActivosAsync(), "Value", "Text", asistenteId);
        }
    }
}
