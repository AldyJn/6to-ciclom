namespace Veterinaria.Entidades.Dtos;

public class UsuarioDto
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }
}
