using System.ComponentModel.DataAnnotations;

namespace Eventos.Web.Models
{
    public class EventoListaVM
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public int Aforo { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public int Inscritos { get; set; }

        [Display(Name = "Cupos disponibles")]
        public int Cupos => Aforo - Inscritos;
    }
}
