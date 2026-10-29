using Eventos.Web.Data;
using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventosConnection")));

builder.Services.AddScoped<EventoRepositorio>();
builder.Services.AddScoped<AsistenteRepositorio>();
builder.Services.AddScoped<InscripcionRepositorio>();

var app = builder.Build();

// Aplica las migraciones y luego carga los datos iniciales (solo si las tablas están vacías)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventosContext>();
    await context.Database.MigrateAsync();

    if (!await context.Categorias.AnyAsync())
    {
        context.Categorias.AddRange(
            new Categoria { Nombre = "Tecnología" },
            new Categoria { Nombre = "Negocios" },
            new Categoria { Nombre = "Cultura" });
        await context.SaveChangesAsync();
    }

    if (!await context.Eventos.AnyAsync())
    {
        var categorias = await context.Categorias.ToDictionaryAsync(c => c.Nombre, c => c.Id);
        var hoy = DateTime.Today;

        context.Eventos.AddRange(
            new Evento { Titulo = "Taller de ASP.NET Core", Fecha = hoy.AddDays(10).AddHours(9), Lugar = "Auditorio Central", Aforo = 40, Precio = 50m, CategoriaId = categorias["Tecnología"], Modalidad = "Presencial" },
            new Evento { Titulo = "Webinar de Emprendimiento", Fecha = hoy.AddDays(15).AddHours(18), Lugar = "Plataforma Zoom", Aforo = 200, Precio = 0m, CategoriaId = categorias["Negocios"], Modalidad = "Virtual" },
            new Evento { Titulo = "Feria del Libro", Fecha = hoy.AddDays(20).AddHours(10), Lugar = "Parque de la Exposición", Aforo = 500, Precio = 10m, CategoriaId = categorias["Cultura"], Modalidad = "Presencial" },
            new Evento { Titulo = "Hackathon de Datos", Fecha = hoy.AddDays(30).AddHours(8), Lugar = "Laboratorio 3", Aforo = 3, Precio = 25.5m, CategoriaId = categorias["Tecnología"], Modalidad = "Presencial" });
        await context.SaveChangesAsync();
    }

    if (!await context.Asistentes.AnyAsync())
    {
        context.Asistentes.AddRange(
            new Asistente { Nombres = "Lucía Fernández", Dni = "70123456", Correo = "lucia.fernandez@correo.com", Telefono = "987654321" },
            new Asistente { Nombres = "Carlos Ramírez", Dni = "70234567", Correo = "carlos.ramirez@correo.com", Telefono = "976543210" },
            new Asistente { Nombres = "Marta Quispe", Dni = "70345678", Correo = "marta.quispe@correo.com" },
            new Asistente { Nombres = "Diego Torres", Dni = "70456789", Correo = "diego.torres@correo.com", Telefono = "965432109" },
            new Asistente { Nombres = "Valeria Soto", Dni = "70567890", Correo = "valeria.soto@correo.com" });
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Eventos}/{action=Index}/{id?}");

app.Run();
