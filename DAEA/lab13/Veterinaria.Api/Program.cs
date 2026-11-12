using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Negocio.Mapeos;
using Veterinaria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<VeterinariaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Veterinaria")));

builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<IMascotaService, MascotaService>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
