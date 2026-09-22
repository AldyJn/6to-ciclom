using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using lab6.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.ViewModels
{
    /// <summary>
    /// Mantenimiento de categorias: alta, edicion y baja logica.
    /// Las tres operaciones de escritura llaman a ExecuteNonQuery en el
    /// repositorio y usan la cantidad de filas afectadas para dar el aviso.
    /// </summary>
    public class CategoriasViewModel : ViewModelBase
    {
        private readonly CategoriaRepositorio _repositorio = new CategoriaRepositorio();
        private Categoria _seleccionada;
        private Categoria _editada = new Categoria();
        private bool _esNueva = true;
        private string _mensaje;

        public CategoriasViewModel()
        {
            Categorias = new ObservableCollection<Categoria>();

            NuevoCommand    = new Comando(Nuevo);
            GuardarCommand  = new Comando(Guardar);
            EliminarCommand = new Comando(Eliminar, () => !_esNueva);

            Refrescar();
        }

        public ObservableCollection<Categoria> Categorias { get; }

        public Categoria Seleccionada
        {
            get => _seleccionada;
            set
            {
                _seleccionada = value;
                Notificar();

                if (value == null) return;

                _esNueva = false;
                Editada = new Categoria
                {
                    CategoriaID     = value.CategoriaID,
                    NombreCategoria = value.NombreCategoria,
                    Descripcion     = value.Descripcion,
                    Activo          = value.Activo
                };
                Mensaje = "Editando la categoria " + value.CategoriaID;
                Notificar(nameof(Titulo));
            }
        }

        /// <summary>Copia de trabajo a la que se enlaza el formulario.</summary>
        public Categoria Editada
        {
            get => _editada;
            set { _editada = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public string Titulo => _esNueva ? "Nueva categoria" : "Editar categoria";

        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private void Refrescar()
        {
            try
            {
                Categorias.Clear();
                foreach (var categoria in _repositorio.Listar()) Categorias.Add(categoria);
                Mensaje = Contar(Categorias.Count, "categoria activa", "categorias activas");
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al listar: " + ex.Message;
            }
        }

        private void Nuevo()
        {
            _esNueva = true;
            Seleccionada = null;
            Editada = new Categoria { Activo = true };
            Mensaje = "Registre una nueva categoria";
            Notificar(nameof(Titulo));
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Editada.NombreCategoria))
            {
                Mensaje = "El nombre de la categoria es obligatorio";
                return;
            }

            try
            {
                // ExecuteNonQuery devuelve la cantidad de filas afectadas
                var filas = _esNueva
                    ? _repositorio.Insertar(Editada)
                    : _repositorio.Actualizar(Editada);

                if (filas == 0)
                {
                    Mensaje = "Ningun registro fue modificado";
                    return;
                }

                var aviso = _esNueva
                    ? "Categoria registrada con el ID " + Editada.CategoriaID
                    : "Categoria " + Editada.CategoriaID + " actualizada";

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
            if (_esNueva || Editada.CategoriaID == 0)
            {
                Mensaje = "Seleccione una categoria de la lista";
                return;
            }

            var respuesta = MessageBox.Show(
                "Se dara de baja la categoria " + Editada.NombreCategoria +
                ".\nLa fila no se borra de la tabla, solo pasa a Activo = 0.",
                "Confirmar baja logica", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                // Baja logica mediante ExecuteNonQuery (UPDATE ... SET Activo = 0)
                var filas = _repositorio.Eliminar(Editada.CategoriaID);

                Mensaje = filas > 0
                    ? "Categoria " + Editada.CategoriaID + " dada de baja (Activo = 0)"
                    : "La categoria ya estaba dada de baja";

                var aviso = Mensaje;
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
