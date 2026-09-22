using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using lab6.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.ViewModels
{
    /// <summary>
    /// Mantenimiento de proveedores con los filtros de busqueda del punto 7.
    /// El listado nunca muestra registros con Activo = 0.
    /// </summary>
    public class ProveedoresViewModel : ViewModelBase
    {
        private readonly ProveedorRepositorio _repositorio = new ProveedorRepositorio();
        private Proveedor _seleccionado;
        private Proveedor _editado = new Proveedor();
        private bool _esNuevo = true;
        private string _filtroContacto;
        private string _filtroCiudad;
        private string _mensaje;

        public ProveedoresViewModel()
        {
            Proveedores = new ObservableCollection<Proveedor>();

            NuevoCommand     = new Comando(Nuevo);
            GuardarCommand   = new Comando(Guardar);
            EliminarCommand  = new Comando(Eliminar, () => !_esNuevo);
            BuscarCommand    = new Comando(Buscar);
            LimpiarCommand   = new Comando(LimpiarFiltros);

            Refrescar();
        }

        public ObservableCollection<Proveedor> Proveedores { get; }

        public Proveedor Seleccionado
        {
            get => _seleccionado;
            set
            {
                _seleccionado = value;
                Notificar();

                if (value == null) return;

                _esNuevo = false;
                Editado = new Proveedor
                {
                    ProveedorID    = value.ProveedorID,
                    CompaniaNombre = value.CompaniaNombre,
                    NombreContacto = value.NombreContacto,
                    CargoContacto  = value.CargoContacto,
                    Direccion      = value.Direccion,
                    Ciudad         = value.Ciudad,
                    CodigoPostal   = value.CodigoPostal,
                    Pais           = value.Pais,
                    Telefono       = value.Telefono,
                    Fax            = value.Fax,
                    Activo         = value.Activo
                };
                Mensaje = "Editando el proveedor " + value.ProveedorID;
                Notificar(nameof(Titulo));
            }
        }

        public Proveedor Editado
        {
            get => _editado;
            set { _editado = value; Notificar(); }
        }

        /// <summary>Filtro por nombre de contacto (busqueda parcial).</summary>
        public string FiltroContacto
        {
            get => _filtroContacto;
            set { _filtroContacto = value; Notificar(); }
        }

        /// <summary>Filtro por ciudad (busqueda parcial).</summary>
        public string FiltroCiudad
        {
            get => _filtroCiudad;
            set { _filtroCiudad = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public string Titulo => _esNuevo ? "Nuevo proveedor" : "Editar proveedor";

        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand BuscarCommand { get; }
        public ICommand LimpiarCommand { get; }

        /// <summary>
        /// Punto 7 y 11.a: ejecuta usp_ProveedoresBuscar con los dos filtros.
        /// Si ambos estan vacios el procedimiento devuelve el listado completo
        /// de proveedores activos.
        /// </summary>
        private void Buscar()
        {
            try
            {
                Proveedores.Clear();
                foreach (var proveedor in _repositorio.Buscar(FiltroContacto, FiltroCiudad))
                    Proveedores.Add(proveedor);

                Mensaje = Proveedores.Count == 0
                    ? "La busqueda no devolvio proveedores activos"
                    : Contar(Proveedores.Count, "proveedor encontrado", "proveedores encontrados");
            }
            catch (SqlException ex)
            {
                Mensaje = "Error en la busqueda: " + ex.Message;
            }
        }

        private void LimpiarFiltros()
        {
            FiltroContacto = null;
            FiltroCiudad = null;
            Refrescar();
        }

        private void Refrescar()
        {
            try
            {
                Proveedores.Clear();
                foreach (var proveedor in _repositorio.Listar()) Proveedores.Add(proveedor);
                Mensaje = Contar(Proveedores.Count, "proveedor activo", "proveedores activos");
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al listar: " + ex.Message;
            }
        }

        private void Nuevo()
        {
            _esNuevo = true;
            Seleccionado = null;
            Editado = new Proveedor { Pais = "Peru", Activo = true };
            Mensaje = "Registre un nuevo proveedor";
            Notificar(nameof(Titulo));
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Editado.CompaniaNombre))
            {
                Mensaje = "El nombre de la compania es obligatorio";
                return;
            }

            try
            {
                // ExecuteNonQuery: alta o edicion segun el estado del formulario
                var filas = _esNuevo
                    ? _repositorio.Insertar(Editado)
                    : _repositorio.Actualizar(Editado);

                if (filas == 0)
                {
                    Mensaje = "Ningun registro fue modificado";
                    return;
                }

                var aviso = _esNuevo
                    ? "Proveedor registrado con el ID " + Editado.ProveedorID
                    : "Proveedor " + Editado.ProveedorID + " actualizado";

                Refrescar();
                Nuevo();
                Mensaje = aviso;
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al guardar: " + ex.Message;
            }
        }

        private void Eliminar()
        {
            if (_esNuevo || Editado.ProveedorID == 0)
            {
                Mensaje = "Seleccione un proveedor de la lista";
                return;
            }

            var respuesta = MessageBox.Show(
                "Se dara de baja al proveedor " + Editado.CompaniaNombre +
                ".\nLa fila no se borra de la tabla, solo pasa a Activo = 0.",
                "Confirmar baja logica", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                // Baja logica mediante ExecuteNonQuery (UPDATE ... SET Activo = 0)
                var filas = _repositorio.Eliminar(Editado.ProveedorID);

                var aviso = filas > 0
                    ? "Proveedor " + Editado.ProveedorID + " dado de baja (Activo = 0)"
                    : "El proveedor ya estaba dado de baja";

                Refrescar();
                Nuevo();
                Mensaje = aviso;
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al eliminar: " + ex.Message;
            }
        }
    }
}
