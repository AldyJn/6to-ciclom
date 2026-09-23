using System.Data;
using System.Threading.Tasks;
using Neptuno.Datos.Modelos;

namespace Neptuno.Datos.Repositorios
{
    public class CategoriaRepositorio : RepositorioBase
    {
        public Task<DataTable> ListarAsync()
            => ConsultarAsync("dbo.usp_CategoriasListar");

        public Task<int> InsertarAsync(Categoria categoria)
            => EjecutarAsync("dbo.usp_CategoriaInsertar",
                ParamTexto("@NombreCategoria", categoria.NombreCategoria),
                ParamTexto("@Descripcion", categoria.Descripcion),
                ParamSalida());

        public Task<int> ActualizarAsync(Categoria categoria)
            => EjecutarAsync("dbo.usp_CategoriaActualizar",
                Param("@CategoriaID", categoria.CategoriaID),
                ParamTexto("@NombreCategoria", categoria.NombreCategoria),
                ParamTexto("@Descripcion", categoria.Descripcion));

        public Task<int> EliminarAsync(int categoriaId)
            => EjecutarAsync("dbo.usp_CategoriaEliminar",
                Param("@CategoriaID", categoriaId));
    }
}
