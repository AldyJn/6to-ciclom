using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Veterinaria.Api.Models;

public partial class Propietario
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = null!;

    [Required]
    [StringLength(8, MinimumLength = 8)]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 digitos")]
    public string Dni { get; set; } = null!;

    [Required]
    [StringLength(15, MinimumLength = 7)]
    public string Telefono { get; set; } = null!;

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [JsonIgnore]
    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}
