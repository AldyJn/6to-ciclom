using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Mantenimientos
{
    public partial class ProductosView : UserControl
    {
        public ProductosView()
        {
            InitializeComponent();
            grid.ItemsSource = Datos.Productos;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text.Trim().Length == 0)
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Datos.Productos.Add(new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Unidad = txtUnidad.Text.Trim()
            });

            txtNombre.Clear();
            txtUnidad.Clear();
        }
    }
}
