namespace Veterinaria.Entidades.Dtos;

public class MascotaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public string? Raza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public decimal Peso { get; set; }

    public string? Microchip { get; set; }

    public bool Activa { get; set; }

    public int PropietarioId { get; set; }

    public string PropietarioNombre { get; set; } = null!;
}
