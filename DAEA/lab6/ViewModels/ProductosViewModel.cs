using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using lab6.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.ViewModels
{
    /// <summary>
    /// Mantenimiento de productos. Insertar y actualizar usan ExecuteNonQuery;
    /// el boton Eliminar invoca usp_ProductoEliminar, que es una baja logica.
    /// </summary>
    public class ProductosViewModel : ViewModelBase
    {
        private readonly ProductoRepositorio _repositorio = new ProductoRepositorio();
        private Producto _seleccionado;
        private Producto _editado = new Producto();
        private bool _esNuevo = true;
        private string _mensaje;

        public ProductosViewModel()
        {
            Productos = new ObservableCollection<Producto>();

            // Los combos solo ofrecen categorias y proveedores vigentes,
            // porque sus listados ya filtran por Activo = 1.
            Categorias = new CategoriaRepositorio().Listar();
            Proveedores = new ProveedorRepositorio().Listar();

            NuevoCommand    = new Comando(Nuevo);
            GuardarCommand  = new Comando(Guardar);
            EliminarCommand = new Comando(Eliminar, () => !_esNuevo);

            Refrescar();
        }

        public ObservableCollection<Producto> Productos { get; }

        public List<Categoria> Categorias { get; }

        public List<Proveedor> Proveedores { get; }

        public Producto Seleccionado
        {
            get => _seleccionado;
            set
            {
                _seleccionado = value;
                Notificar();

                if (value == null) return;

                _esNuevo = false;
                Editado = new Producto
                {
                    ProductoID           = value.ProductoID,
                    NombreProducto       = value.NombreProducto,
                    ProveedorID          = value.ProveedorID,
                    CategoriaID          = value.CategoriaID,
                    CantidadPorUnidad    = value.CantidadPorUnidad,
                    PrecioUnidad         = value.PrecioUnidad,
                    UnidadesEnExistencia = value.UnidadesEnExistencia,
                    UnidadesEnPedido     = value.UnidadesEnPedido,
                    NivelDeReorden       = value.NivelDeReorden,
                    Descontinuado        = value.Descontinuado,
                    Activo               = value.Activo
                };
                Mensaje = "Editando el producto " + value.ProductoID;
                Notificar(nameof(Titulo));
            }
        }

        public Producto Editado
        {
            get => _editado;
            set { _editado = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public string Titulo => _esNuevo ? "Nuevo producto" : "Editar producto";

        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private void Refrescar()
        {
            try
            {
                Productos.Clear();
                foreach (var producto in _repositorio.Listar()) Productos.Add(producto);
                Mensaje = Contar(Productos.Count, "producto activo", "productos activos");
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
            Editado = new Producto { Activo = true };
            Mensaje = "Registre un nuevo producto";
            Notificar(nameof(Titulo));
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Editado.NombreProducto))
            {
                Mensaje = "El nombre del producto es obligatorio";
                return;
            }

            if (Editado.PrecioUnidad < 0)
            {
                Mensaje = "El precio unitario no puede ser negativo";
                return;
            }

            try
            {
                // ExecuteNonQuery devuelve las filas afectadas por el INSERT o el UPDATE
                var filas = _esNuevo
                    ? _repositorio.Insertar(Editado)
                    : _repositorio.Actualizar(Editado);

                if (filas == 0)
                {
                    Mensaje = "Ningun registro fue modificado";
                    return;
                }

                var aviso = _esNuevo
                    ? "Producto registrado con el ID " + Editado.ProductoID
                    : "Producto " + Editado.ProductoID + " actualizado";

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
            if (_esNuevo || Editado.ProductoID == 0)
            {
                Mensaje = "Seleccione un producto de la lista";
                return;
            }

            var respuesta = MessageBox.Show(
                "Se dara de baja el producto " + Editado.NombreProducto +
                ".\nLa fila no se borra de la tabla, solo pasa a Activo = 0.",
                "Confirmar baja logica", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                // Baja logica mediante ExecuteNonQuery (UPDATE ... SET Activo = 0)
                var filas = _repositorio.Eliminar(Editado.ProductoID);

                var aviso = filas > 0
                    ? "Producto " + Editado.ProductoID + " dado de baja (Activo = 0)"
                    : "El producto ya estaba dado de baja";

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
