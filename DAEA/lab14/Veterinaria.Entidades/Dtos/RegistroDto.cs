using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
    [StringLength(100)]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La clave es obligatoria")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La clave debe tener entre 8 y 100 caracteres")]
    public string Clave { get; set; } = null!;
}
