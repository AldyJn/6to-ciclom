using System.ComponentModel.DataAnnotations;

namespace Semana09.Caso4.Models;

public class Cita
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del paciente es obligatorio")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 80 caracteres")]
    public string Paciente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo no válido")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Seleccione una especialidad")]
    public string Especialidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha y hora")]
    public DateTime Fecha { get; set; } = DateTime.Today.AddDays(1).AddHours(9);

    [Display(Name = "¿Primera vez?")]
    public bool PrimeraVez { get; set; }
}
