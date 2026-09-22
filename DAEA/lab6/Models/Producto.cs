namespace lab6.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public int? ProveedorID { get; set; }
        public string Proveedor { get; set; }
        public int? CategoriaID { get; set; }
        public string Categoria { get; set; }
        public string CantidadPorUnidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short UnidadesEnExistencia { get; set; }
        public short UnidadesEnPedido { get; set; }
        public short NivelDeReorden { get; set; }
        public bool Descontinuado { get; set; }
        public bool Activo { get; set; }

        public override string ToString() => NombreProducto;
    }
}
