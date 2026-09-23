using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Neptuno.Datos.Modelos;
using Neptuno.Datos.Repositorios;

namespace Neptuno.App.Vistas
{
    public partial class CategoriasView : UserControl
    {
        private readonly CategoriaRepositorio repositorio = new CategoriaRepositorio();
        private int categoriaId;

        public CategoriasView()
        {
            InitializeComponent();
            Loaded += CategoriasView_Loaded;
        }

        private async void CategoriasView_Loaded(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            var categoria = new Categoria
            {
                CategoriaID = categoriaId,
                NombreCategoria = txtNombre.Text,
                Descripcion = txtDescripcion.Text
            };

            try
            {
                if (categoriaId == 0)
                    await repositorio.InsertarAsync(categoria);
                else
                    await repositorio.ActualizarAsync(categoria);

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
            if (categoriaId == 0)
            {
                MessageBox.Show("Seleccione una categoria.");
                return;
            }

            var respuesta = MessageBox.Show(
                "La categoria quedara con Activo = 0. Continuar?",
                "Eliminacion logica", MessageBoxButton.YesNo);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                await repositorio.EliminarAsync(categoriaId);
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

            categoriaId = Convert.ToInt32(fila["CategoriaID"]);
            txtNombre.Text = fila["NombreCategoria"].ToString();
            txtDescripcion.Text = fila["Descripcion"].ToString();
            lblEstado.Text = "Editando la categoria " + categoriaId;
        }

        private async Task CargarAsync()
        {
            try
            {
                var tabla = await repositorio.ListarAsync();
                grilla.ItemsSource = tabla.DefaultView;
                lblEstado.Text = tabla.Rows.Count + " categorias activas";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            categoriaId = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            lblEstado.Text = "Nueva categoria";
        }
    }
}
