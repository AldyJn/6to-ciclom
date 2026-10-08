using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título admite como máximo 150 caracteres.")]
    [DataType(DataType.Text)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 20 caracteres.")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Seleccione un autor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un autor.")]
    [Display(Name = "Autor")]
    public int? AutorId { get; set; }

    [Display(Name = "Autor")]
    public string? AutorNombre { get; set; }

    [Required(ErrorMessage = "Ingrese la cantidad de ejemplares.")]
    [Range(0, 1000, ErrorMessage = "Los ejemplares deben estar entre 0 y 1000.")]
    public int? Ejemplares { get; set; }

    public bool Activo { get; set; } = true;
}
