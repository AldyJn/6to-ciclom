namespace lab3.Models
{
    public class Aula
    {
        public int AulaId { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }

        public override string ToString() => Nombre;
    }
}
