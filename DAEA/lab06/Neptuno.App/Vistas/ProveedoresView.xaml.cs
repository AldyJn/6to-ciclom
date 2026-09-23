using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Neptuno.Datos.Modelos;
using Neptuno.Datos.Repositorios;

namespace Neptuno.App.Vistas
{
    public partial class ProveedoresView : UserControl
    {
        private readonly ProveedorRepositorio repositorio = new ProveedorRepositorio();
        private int proveedorId;

        public ProveedoresView()
        {
            InitializeComponent();
            Loaded += ProveedoresView_Loaded;
        }

        private async void ProveedoresView_Loaded(object sender, RoutedEventArgs e)
        {
            await BuscarAsync();
        }

        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            await BuscarAsync();
        }

        private async void LimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txtFiltroContacto.Clear();
            txtFiltroCiudad.Clear();
            await BuscarAsync();
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompania.Text))
            {
                MessageBox.Show("El nombre de la compania es obligatorio.");
                return;
            }

            var proveedor = new Proveedor
            {
                ProveedorID = proveedorId,
                CompaniaNombre = txtCompania.Text,
                NombreContacto = txtContacto.Text,
                CargoContacto = txtCargo.Text,
                Direccion = txtDireccion.Text,
                Ciudad = txtCiudad.Text,
                Pais = txtPais.Text,
                Telefono = txtTelefono.Text
            };

            try
            {
                if (proveedorId == 0)
                    await repositorio.InsertarAsync(proveedor);
                else
                    await repositorio.ActualizarAsync(proveedor);

                Limpiar();
                await BuscarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (proveedorId == 0)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            var respuesta = MessageBox.Show(
                "El proveedor quedara con Activo = 0. Continuar?",
                "Eliminacion logica", MessageBoxButton.YesNo);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                await repositorio.EliminarAsync(proveedorId);
                Limpiar();
                await BuscarAsync();
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

            proveedorId = Convert.ToInt32(fila["ProveedorID"]);
            txtCompania.Text = fila["CompaniaNombre"].ToString();
            txtContacto.Text = fila["NombreContacto"].ToString();
            txtCargo.Text = fila["CargoContacto"].ToString();
            txtDireccion.Text = fila["Direccion"].ToString();
            txtCiudad.Text = fila["Ciudad"].ToString();
            txtPais.Text = fila["Pais"].ToString();
            txtTelefono.Text = fila["Telefono"].ToString();
            lblEstado.Text = "Editando el proveedor " + proveedorId;
        }

        private async Task BuscarAsync()
        {
            try
            {
                var tabla = await repositorio.BuscarAsync(txtFiltroContacto.Text, txtFiltroCiudad.Text);
                grilla.ItemsSource = tabla.DefaultView;
                lblEstado.Text = tabla.Rows.Count + " proveedores encontrados";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            proveedorId = 0;
            txtCompania.Clear();
            txtContacto.Clear();
            txtCargo.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtPais.Clear();
            txtTelefono.Clear();
            lblEstado.Text = "Nuevo proveedor";
        }
    }
}
