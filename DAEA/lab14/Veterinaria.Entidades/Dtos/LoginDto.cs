using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La clave es obligatoria")]
    public string Clave { get; set; } = null!;
}
