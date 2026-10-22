using Hotel.Web.Models;
using Hotel.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel.Web.Controllers;

public class HabitacionesController : Controller
{
    private readonly HabitacionRepositorio _repositorio;

    public HabitacionesController(HabitacionRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IActionResult> Index(int? idTipo)
    {
        await CargarTiposAsync(idTipo);
        var habitaciones = await _repositorio.ListarActivasAsync(idTipo);
        return View(habitaciones);
    }

    public async Task<IActionResult> Details(int id)
    {
        var habitacion = await _repositorio.ObtenerAsync(id);
        if (habitacion == null)
        {
            return NotFound();
        }
        return View(habitacion);
    }

    public async Task<IActionResult> Create()
    {
        await CargarTiposAsync(null);
        return View(new Habitacion { Piso = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Habitacion habitacion)
    {
        ModelState.Remove(nameof(Habitacion.IdTipoHabitacionNavigation));
        ModelState.Remove(nameof(Habitacion.Reservas));
        await ValidarAsync(habitacion);

        if (ModelState.IsValid)
        {
            await _repositorio.CrearAsync(habitacion);
            TempData["Mensaje"] = $"La habitación {habitacion.Numero} se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarTiposAsync(habitacion.IdTipoHabitacion);
        return View(habitacion);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var habitacion = await _repositorio.ObtenerAsync(id);
        if (habitacion == null)
        {
            return NotFound();
        }
        await CargarTiposAsync(habitacion.IdTipoHabitacion);
        return View(habitacion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Habitacion habitacion)
    {
        if (id != habitacion.IdHabitacion)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Habitacion.IdTipoHabitacionNavigation));
        ModelState.Remove(nameof(Habitacion.Reservas));
        await ValidarAsync(habitacion);

        if (ModelState.IsValid)
        {
            var actualizada = await _repositorio.ActualizarAsync(habitacion);
            if (!actualizada)
            {
                return NotFound();
            }
            TempData["Mensaje"] = $"La habitación {habitacion.Numero} se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarTiposAsync(habitacion.IdTipoHabitacion);
        return View(habitacion);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var habitacion = await _repositorio.ObtenerAsync(id);
        if (habitacion == null)
        {
            return NotFound();
        }
        return View(habitacion);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eliminada = await _repositorio.EliminarLogicoAsync(id);
        if (eliminada)
        {
            TempData["Mensaje"] = "La habitación se dio de baja correctamente.";
        }
        else
        {
            TempData["Error"] = "No se encontró la habitación indicada.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarAsync(Habitacion habitacion)
    {
        if (string.IsNullOrWhiteSpace(habitacion.Numero))
        {
            return;
        }

        habitacion.Numero = habitacion.Numero.Trim();

        if (habitacion.Numero.Length > 10)
        {
            ModelState.AddModelError(nameof(Habitacion.Numero), "El número no puede superar los 10 caracteres.");
        }
        else if (await _repositorio.ExisteNumeroAsync(habitacion.Numero, habitacion.IdHabitacion))
        {
            ModelState.AddModelError(nameof(Habitacion.Numero), $"Ya existe una habitación con el número {habitacion.Numero}.");
        }

        if (habitacion.Piso < 1)
        {
            ModelState.AddModelError(nameof(Habitacion.Piso), "El piso debe ser mayor o igual a 1.");
        }

        if (habitacion.IdTipoHabitacion == 0)
        {
            ModelState.AddModelError(nameof(Habitacion.IdTipoHabitacion), "Seleccione un tipo de habitación.");
        }
    }

    private async Task CargarTiposAsync(int? seleccionado)
    {
        var tipos = await _repositorio.ListarTiposAsync();
        ViewBag.Tipos = new SelectList(tipos, nameof(TipoHabitacion.IdTipoHabitacion), nameof(TipoHabitacion.Nombre), seleccionado);
    }
}
