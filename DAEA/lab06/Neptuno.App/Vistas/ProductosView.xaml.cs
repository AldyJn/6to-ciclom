using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Neptuno.Datos.Modelos;
using Neptuno.Datos.Repositorios;

namespace Neptuno.App.Vistas
{
    public partial class ProductosView : UserControl
    {
        private readonly ProductoRepositorio repositorio = new ProductoRepositorio();
        private readonly CategoriaRepositorio categorias = new CategoriaRepositorio();
        private readonly ProveedorRepositorio proveedores = new ProveedorRepositorio();
        private int productoId;

        public ProductosView()
        {
            InitializeComponent();
            Loaded += ProductosView_Loaded;
        }

        private async void ProductosView_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarCombosAsync();
            await CargarAsync();
        }

        private async void Refrescar_Click(object sender, RoutedEventArgs e)
        {
            await CargarAsync();
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out var precio) || precio < 0)
            {
                MessageBox.Show("El precio no es valido.");
                return;
            }

            if (!short.TryParse(txtExistencia.Text, out var existencia) || existencia < 0)
            {
                MessageBox.Show("Las unidades en existencia no son validas.");
                return;
            }

            var producto = new Producto
            {
                ProductoID = productoId,
                NombreProducto = txtNombre.Text,
                CategoriaID = (int?)cboCategoria.SelectedValue,
                ProveedorID = (int?)cboProveedor.SelectedValue,
                CantidadPorUnidad = txtCantidadPorUnidad.Text,
                PrecioUnidad = precio,
                UnidadesEnExistencia = existencia,
                UnidadesEnPedido = 0,
                NivelDeReorden = 0,
                Descontinuado = chkDescontinuado.IsChecked == true
            };

            try
            {
                if (productoId == 0)
                    await repositorio.InsertarAsync(producto);
                else
                    await repositorio.ActualizarAsync(producto);

                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (productoId == 0)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            var respuesta = MessageBox.Show(
                "El producto quedara con Activo = 0. Continuar?",
                "Eliminacion logica", MessageBoxButton.YesNo);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                await repositorio.EliminarAsync(productoId);
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            grilla.SelectedItem = null;
            Limpiar();
        }

        private void Grilla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(grilla.SelectedItem is DataRowView fila)) return;

            productoId = Convert.ToInt32(fila["ProductoID"]);
            txtNombre.Text = fila["NombreProducto"].ToString();
            cboCategoria.SelectedValue = fila["CategoriaID"] == DBNull.Value ? null : fila["CategoriaID"];
            cboProveedor.SelectedValue = fila["ProveedorID"] == DBNull.Value ? null : fila["ProveedorID"];
            txtCantidadPorUnidad.Text = fila["CantidadPorUnidad"].ToString();
            txtPrecio.Text = Convert.ToDecimal(fila["PrecioUnidad"]).ToString("0.00");
            txtExistencia.Text = fila["UnidadesEnExistencia"].ToString();
            chkDescontinuado.IsChecked = Convert.ToBoolean(fila["Descontinuado"]);
            lblEstado.Text = "Editando el producto " + productoId;
        }

        private async Task CargarAsync()
        {
            try
            {
                var tabla = await repositorio.ListarAsync();
                grilla.ItemsSource = tabla.DefaultView;
                lblEstado.Text = tabla.Rows.Count + " productos activos";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task CargarCombosAsync()
        {
            try
            {
                cboCategoria.ItemsSource = (await categorias.ListarAsync()).DefaultView;
                cboProveedor.ItemsSource = (await proveedores.ListarAsync()).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            productoId = 0;
            txtNombre.Clear();
            txtCantidadPorUnidad.Clear();
            txtPrecio.Text = "0";
            txtExistencia.Text = "0";
            cboCategoria.SelectedIndex = -1;
            cboProveedor.SelectedIndex = -1;
            chkDescontinuado.IsChecked = false;
            lblEstado.Text = "Nuevo producto";
        }
    }
}
