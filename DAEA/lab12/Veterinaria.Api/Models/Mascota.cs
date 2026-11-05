using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Api.Models;

public partial class Mascota
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Nombre { get; set; } = null!;

    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Especie { get; set; } = null!;

    [StringLength(60)]
    public string? Raza { get; set; }

    [Range(0.01, 200)]
    public decimal PesoKg { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public bool Activa { get; set; }

    [Range(1, int.MaxValue)]
    public int PropietarioId { get; set; }

    public virtual Propietario? Propietario { get; set; }
}
