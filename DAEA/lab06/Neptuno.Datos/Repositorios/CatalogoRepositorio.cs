using System.Data;
using System.Threading.Tasks;

namespace Neptuno.Datos.Repositorios
{
    public class CatalogoRepositorio : RepositorioBase
    {
        public Task<DataTable> ClientesAsync()
            => ConsultarTextoAsync("SELECT ClienteID, Empresa FROM dbo.Clientes ORDER BY Empresa");

        public Task<DataTable> EmpleadosAsync()
            => ConsultarTextoAsync("SELECT EmpleadoID, Nombre + N' ' + Apellidos AS Empleado FROM dbo.Empleados ORDER BY Apellidos");

        public Task<DataTable> TransportistasAsync()
            => ConsultarTextoAsync("SELECT TransportistaID, CompaniaNombre FROM dbo.Transportistas ORDER BY CompaniaNombre");
    }
}
