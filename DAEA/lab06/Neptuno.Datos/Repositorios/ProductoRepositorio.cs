using System.Data;
using System.Threading.Tasks;
using Neptuno.Datos.Modelos;

namespace Neptuno.Datos.Repositorios
{
    public class ProductoRepositorio : RepositorioBase
    {
        public Task<DataTable> ListarAsync()
            => ConsultarAsync("dbo.usp_ProductosListar");

        public Task<int> InsertarAsync(Producto producto)
            => EjecutarAsync("dbo.usp_ProductoInsertar",
                ParamTexto("@NombreProducto", producto.NombreProducto),
                Param("@ProveedorID", producto.ProveedorID),
                Param("@CategoriaID", producto.CategoriaID),
                ParamTexto("@CantidadPorUnidad", producto.CantidadPorUnidad),
                Param("@PrecioUnidad", producto.PrecioUnidad),
                Param("@UnidadesEnExistencia", producto.UnidadesEnExistencia),
                Param("@UnidadesEnPedido", producto.UnidadesEnPedido),
                Param("@NivelDeReorden", producto.NivelDeReorden),
                Param("@Descontinuado", producto.Descontinuado),
                ParamSalida());

        public Task<int> ActualizarAsync(Producto producto)
            => EjecutarAsync("dbo.usp_ProductoActualizar",
                Param("@ProductoID", producto.ProductoID),
                ParamTexto("@NombreProducto", producto.NombreProducto),
                Param("@ProveedorID", producto.ProveedorID),
                Param("@CategoriaID", producto.CategoriaID),
                ParamTexto("@CantidadPorUnidad", producto.CantidadPorUnidad),
                Param("@PrecioUnidad", producto.PrecioUnidad),
                Param("@UnidadesEnExistencia", producto.UnidadesEnExistencia),
                Param("@UnidadesEnPedido", producto.UnidadesEnPedido),
                Param("@NivelDeReorden", producto.NivelDeReorden),
                Param("@Descontinuado", producto.Descontinuado));

        public Task<int> EliminarAsync(int productoId)
            => EjecutarAsync("dbo.usp_ProductoEliminar",
                Param("@ProductoID", productoId));
    }
}
