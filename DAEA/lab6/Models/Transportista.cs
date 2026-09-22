namespace lab6.Models
{
    public class Transportista
    {
        public int TransportistaID { get; set; }
        public string CompaniaNombre { get; set; }

        public override string ToString() => CompaniaNombre;
    }
}
