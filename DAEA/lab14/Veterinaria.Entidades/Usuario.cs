namespace Veterinaria.Entidades;

public class Usuario
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    public string ClaveHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }
}
