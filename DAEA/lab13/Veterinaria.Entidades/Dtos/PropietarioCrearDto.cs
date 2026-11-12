using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class PropietarioCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El documento es obligatorio")]
    [StringLength(20, MinimumLength = 8)]
    public string Documento { get; set; } = null!;

    [StringLength(20)]
    public string? Telefono { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }
}
