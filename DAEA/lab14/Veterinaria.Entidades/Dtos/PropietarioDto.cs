namespace Veterinaria.Entidades.Dtos;

public class PropietarioDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int TotalMascotas { get; set; }
}
