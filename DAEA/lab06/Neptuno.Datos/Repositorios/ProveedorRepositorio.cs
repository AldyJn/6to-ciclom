using System.Data;
using System.Threading.Tasks;
using Neptuno.Datos.Modelos;

namespace Neptuno.Datos.Repositorios
{
    public class ProveedorRepositorio : RepositorioBase
    {
        public Task<DataTable> ListarAsync()
            => ConsultarAsync("dbo.usp_ProveedoresListar");

        public Task<DataTable> BuscarAsync(string nombreContacto, string ciudad)
            => ConsultarAsync("dbo.usp_ProveedoresBuscar",
                ParamTexto("@NombreContacto", nombreContacto),
                ParamTexto("@Ciudad", ciudad));

        public Task<int> InsertarAsync(Proveedor proveedor)
            => EjecutarAsync("dbo.usp_ProveedorInsertar",
                ParamTexto("@CompaniaNombre", proveedor.CompaniaNombre),
                ParamTexto("@NombreContacto", proveedor.NombreContacto),
                ParamTexto("@CargoContacto", proveedor.CargoContacto),
                ParamTexto("@Direccion", proveedor.Direccion),
                ParamTexto("@Ciudad", proveedor.Ciudad),
                ParamTexto("@CodigoPostal", proveedor.CodigoPostal),
                ParamTexto("@Pais", proveedor.Pais),
                ParamTexto("@Telefono", proveedor.Telefono),
                ParamTexto("@Fax", proveedor.Fax),
                ParamSalida());

        public Task<int> ActualizarAsync(Proveedor proveedor)
            => EjecutarAsync("dbo.usp_ProveedorActualizar",
                Param("@ProveedorID", proveedor.ProveedorID),
                ParamTexto("@CompaniaNombre", proveedor.CompaniaNombre),
                ParamTexto("@NombreContacto", proveedor.NombreContacto),
                ParamTexto("@CargoContacto", proveedor.CargoContacto),
                ParamTexto("@Direccion", proveedor.Direccion),
                ParamTexto("@Ciudad", proveedor.Ciudad),
                ParamTexto("@CodigoPostal", proveedor.CodigoPostal),
                ParamTexto("@Pais", proveedor.Pais),
                ParamTexto("@Telefono", proveedor.Telefono),
                ParamTexto("@Fax", proveedor.Fax));

        public Task<int> EliminarAsync(int proveedorId)
            => EjecutarAsync("dbo.usp_ProveedorEliminar",
                Param("@ProveedorID", proveedorId));
    }
}
