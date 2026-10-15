using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Semana09.Caso4.Models;

namespace Semana09.Caso4.Controllers;

public class CitasController : Controller
{
    private static readonly List<Cita> _citas = new()
    {
        new Cita { Id = 1, Paciente = "Ana Torres", Correo = "ana@clinica.pe", Especialidad = "Cardiología", Fecha = DateTime.Today.AddDays(2).AddHours(10), PrimeraVez = true },
        new Cita { Id = 2, Paciente = "Luis Rojas", Correo = "luis@clinica.pe", Especialidad = "Pediatría", Fecha = DateTime.Today.AddDays(3).AddHours(15), PrimeraVez = false }
    };
    private static int _nextId = 3;

    private static readonly string[] _especialidades =
        { "Medicina General", "Cardiología", "Pediatría", "Dermatología", "Traumatología" };

    private void CargarEspecialidades() =>
        ViewBag.Especialidades = new SelectList(_especialidades);

    public IActionResult Index() => View(_citas.OrderBy(c => c.Fecha).ToList());

    public IActionResult CreateHelpers()
    {
        CargarEspecialidades();
        return View(new Cita());
    }

    public IActionResult CreateTagHelpers()
    {
        CargarEspecialidades();
        return View(new Cita());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Cita cita, string vista)
    {
        if (!ModelState.IsValid)
        {
            CargarEspecialidades();
            return View(vista == "CreateHelpers" ? "CreateHelpers" : "CreateTagHelpers", cita);
        }

        cita.Id = _nextId++;
        _citas.Add(cita);
        TempData["Mensaje"] = $"Cita registrada para {cita.Paciente}";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        return cita is null ? NotFound() : View(cita);
    }
}
