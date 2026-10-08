using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly SocioRepositorio _socios;

    public SociosController(SocioRepositorio socios)
    {
        _socios = socios;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _socios.ListarAsync());
    }

    public IActionResult Create()
    {
        return View(new Socio());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _socios.InsertarAsync(socio);
                TempData["Mensaje"] = $"El socio {socio.Nombre} se registró correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                ModelState.AddModelError(nameof(Socio.DNI), "Ya existe un socio registrado con ese DNI.");
            }
        }
        return View(socio);
    }
}
