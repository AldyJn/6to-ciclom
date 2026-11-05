using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Veterinaria")
    ?? throw new InvalidOperationException("Falta la cadena de conexion ConnectionStrings__Veterinaria");
builder.Services.AddDbContext<VeterinariaContext>(o => o.UseNpgsql(connectionString));

var origen = builder.Configuration["Cors:Origen"] ?? "http://localhost:3000";
builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
    p.WithOrigins(origen).AllowAnyHeader().AllowAnyMethod()));

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownIPNetworks.Clear();
    o.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();
app.MapOpenApi();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1");
    o.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
