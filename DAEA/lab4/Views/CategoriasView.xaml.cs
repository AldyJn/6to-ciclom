using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using lab4.Data;

namespace lab4.Views
{
    public partial class CategoriasView : UserControl
    {
        public CategoriasView()
        {
            InitializeComponent();
            Cargar();
        }

        private void Cargar()
        {
            try { grid.ItemsSource = Db.Consultar("usp_CategoriasListar").DefaultView; }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Limpiar()
        {
            grid.SelectedItem = null;
            txtId.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtNombre.Focus();
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Ui.Columnas(e, "CategoriaID");
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is DataRowView fila)
            {
                txtId.Text = Ui.Texto(fila["CategoriaID"]);
                txtNombre.Text = Ui.Texto(fila["NombreCategoria"]);
                txtDescripcion.Text = Ui.Texto(fila["Descripcion"]);
            }
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                Ui.Info("Ingrese el nombre de la categoría.");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                    Db.Ejecutar("usp_CategoriaInsertar",
                        Db.P("@NombreCategoria", txtNombre.Text.Trim()),
                        Db.P("@Descripcion", txtDescripcion.Text));
                else
                    Db.Ejecutar("usp_CategoriaActualizar",
                        Db.P("@CategoriaID", int.Parse(txtId.Text)),
                        Db.P("@NombreCategoria", txtNombre.Text.Trim()),
                        Db.P("@Descripcion", txtDescripcion.Text));
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                Ui.Info("Seleccione una categoría.");
                return;
            }
            if (!Ui.Confirmar("¿Eliminar la categoría seleccionada?")) return;
            try
            {
                Db.Ejecutar("usp_CategoriaEliminar", Db.P("@CategoriaID", int.Parse(txtId.Text)));
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }
    }
}
