namespace lab6.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string Empresa { get; set; }

        public override string ToString() => Empresa;
    }
}
