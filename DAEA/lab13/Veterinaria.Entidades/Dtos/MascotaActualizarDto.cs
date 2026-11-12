using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class MascotaActualizarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80, MinimumLength = 2)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La especie es obligatoria")]
    [StringLength(30)]
    public string Especie { get; set; } = null!;

    [StringLength(60)]
    public string? Raza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    [Range(0.01, 200, ErrorMessage = "El peso debe estar entre 0.01 y 200 kg")]
    public decimal Peso { get; set; }

    [StringLength(30)]
    public string? Microchip { get; set; }

    public bool Activa { get; set; }
}
