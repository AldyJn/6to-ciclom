using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly PrestamoRepositorio _prestamos;

    public PrestamosController(PrestamoRepositorio prestamos)
    {
        _prestamos = prestamos;
    }

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var fin = hasta ?? DateTime.Today;
        var inicio = desde ?? fin.AddMonths(-3);

        ViewData["Desde"] = inicio.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fin.ToString("yyyy-MM-dd");

        if (inicio > fin)
        {
            ModelState.AddModelError(string.Empty, "La fecha inicial no puede ser mayor que la fecha final.");
            return View(Enumerable.Empty<PrestamoReporte>());
        }

        return View(await _prestamos.ReportePorFechasAsync(inicio, fin));
    }
}
