using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _libros;

    public LibrosController(LibroRepositorio libros)
    {
        _libros = libros;
    }

    public async Task<IActionResult> Index(string? titulo)
    {
        var lista = string.IsNullOrWhiteSpace(titulo)
            ? await _libros.ListarAsync()
            : await _libros.BuscarPorTituloAsync(titulo.Trim());
        ViewData["Busqueda"] = titulo;
        return View(lista);
    }

    public async Task<IActionResult> Details(int id)
    {
        var libro = await _libros.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        return View(libro);
    }

    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync(null);
        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _libros.InsertarAsync(libro);
                TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se registró correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe un libro con ese ISBN.");
            }
        }
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _libros.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.LibroId) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                var actualizado = await _libros.ActualizarAsync(libro);
                if (!actualizado) return NotFound();
                TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe otro libro con ese ISBN.");
            }
        }
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _libros.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        return View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eliminado = await _libros.EliminarAsync(id);
        if (eliminado)
            TempData["Mensaje"] = "El libro se eliminó correctamente.";
        else
            TempData["Error"] = "No se encontró el libro que desea eliminar.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? seleccionado)
    {
        var autores = await _libros.ListarAutoresAsync();
        ViewBag.Autores = new SelectList(autores, "AutorId", "Nombre", seleccionado);
    }
}
