namespace lab6.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string NombreCompleto { get; set; }

        public override string ToString() => NombreCompleto;
    }
}
