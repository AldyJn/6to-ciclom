namespace lab6.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public override string ToString() => NombreCategoria;
    }
}
