using Hotel.Web.Models;
using Hotel.Web.Repositorios;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel.Web.Controllers;

public class ReservasController : Controller
{
    private readonly ReservaRepositorio _repositorio;

    public ReservasController(ReservaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IActionResult> Index()
    {
        var reservas = await _repositorio.ListarAsync();
        return View(reservas);
    }

    public async Task<IActionResult> Disponibles(DateOnly? ingreso, DateOnly? salida)
    {
        var modelo = new DisponiblesViewModel { Ingreso = ingreso, Salida = salida };

        if (ingreso.HasValue && salida.HasValue)
        {
            modelo.Buscado = true;

            if (salida.Value <= ingreso.Value)
            {
                ModelState.AddModelError(nameof(DisponiblesViewModel.Salida), "La fecha de salida debe ser posterior a la fecha de ingreso.");
            }
            else
            {
                var noches = salida.Value.DayNumber - ingreso.Value.DayNumber;
                var habitaciones = await _repositorio.DisponiblesAsync(ingreso.Value, salida.Value);
                modelo.Items = habitaciones.Select(h => new DisponibleItem
                {
                    IdHabitacion = h.IdHabitacion,
                    Numero = h.Numero,
                    Piso = h.Piso,
                    Tipo = h.IdTipoHabitacionNavigation.Nombre,
                    Capacidad = h.IdTipoHabitacionNavigation.Capacidad,
                    PrecioNoche = h.IdTipoHabitacionNavigation.PrecioNoche,
                    Noches = noches,
                    Total = noches * h.IdTipoHabitacionNavigation.PrecioNoche
                }).ToList();
            }
        }

        return View(modelo);
    }

    public async Task<IActionResult> Create(int? idHabitacion, DateOnly? ingreso, DateOnly? salida)
    {
        var reserva = new Reserva
        {
            FechaIngreso = ingreso ?? DateOnly.FromDateTime(DateTime.Today),
            FechaSalida = salida ?? DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            IdHabitacion = idHabitacion ?? 0
        };
        await CargarListasAsync(reserva);
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reserva reserva)
    {
        ModelState.Remove(nameof(Reserva.IdHuespedNavigation));
        ModelState.Remove(nameof(Reserva.IdHabitacionNavigation));
        ModelState.Remove(nameof(Reserva.Estado));

        if (reserva.IdHuesped == 0)
        {
            ModelState.AddModelError(nameof(Reserva.IdHuesped), "Seleccione un huésped.");
        }

        if (reserva.IdHabitacion == 0)
        {
            ModelState.AddModelError(nameof(Reserva.IdHabitacion), "Seleccione una habitación.");
        }

        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (reserva.FechaSalida <= reserva.FechaIngreso)
        {
            ModelState.AddModelError(nameof(Reserva.FechaSalida), "La fecha de salida debe ser posterior a la fecha de ingreso.");
        }

        if (reserva.FechaIngreso < hoy)
        {
            ModelState.AddModelError(nameof(Reserva.FechaIngreso), "La fecha de ingreso no puede ser anterior a hoy.");
        }

        if (!string.IsNullOrWhiteSpace(reserva.Observaciones) && reserva.Observaciones.Length > 200)
        {
            ModelState.AddModelError(nameof(Reserva.Observaciones), "Las observaciones no pueden superar los 200 caracteres.");
        }

        if (ModelState.IsValid
            && !await _repositorio.HabitacionLibreAsync(reserva.IdHabitacion, reserva.FechaIngreso, reserva.FechaSalida))
        {
            ModelState.AddModelError(nameof(Reserva.IdHabitacion), "La habitación ya está reservada en el rango de fechas seleccionado.");
        }

        if (ModelState.IsValid)
        {
            reserva.Observaciones = string.IsNullOrWhiteSpace(reserva.Observaciones) ? null : reserva.Observaciones.Trim();
            await _repositorio.CrearAsync(reserva);
            TempData["Mensaje"] = "La reserva se registró con estado Pendiente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarListasAsync(reserva);
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var cancelada = await _repositorio.CancelarAsync(id);
        if (cancelada)
        {
            TempData["Mensaje"] = "La reserva fue cancelada.";
        }
        else
        {
            TempData["Error"] = "Solo se pueden cancelar reservas Pendientes o Confirmadas.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(Reserva reserva)
    {
        var huespedes = await _repositorio.ListarHuespedesActivosAsync();
        var habitaciones = await _repositorio.ListarHabitacionesActivasAsync();

        ViewBag.Huespedes = huespedes.Select(h => new SelectListItem
        {
            Value = h.IdHuesped.ToString(),
            Text = $"{h.Apellidos}, {h.Nombres} ({h.Dni})",
            Selected = h.IdHuesped == reserva.IdHuesped
        }).ToList();

        ViewBag.Habitaciones = habitaciones.Select(h => new SelectListItem
        {
            Value = h.IdHabitacion.ToString(),
            Text = $"{h.Numero} · {h.IdTipoHabitacionNavigation.Nombre} · S/ {h.IdTipoHabitacionNavigation.PrecioNoche:N2} por noche",
            Selected = h.IdHabitacion == reserva.IdHabitacion
        }).ToList();
    }
}
