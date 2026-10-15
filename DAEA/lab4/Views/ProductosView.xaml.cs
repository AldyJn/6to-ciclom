using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using lab4.Data;

namespace lab4.Views
{
    public partial class ProductosView : UserControl
    {
        public ProductosView()
        {
            InitializeComponent();
            try
            {
                Ui.Combo(cboProveedor, "usp_ProveedoresCombo");
                Ui.Combo(cboCategoria, "usp_CategoriasCombo");
            }
            catch (Exception ex) { Ui.Error(ex); }
            Cargar();
        }

        private void Cargar()
        {
            try { grid.ItemsSource = Db.Consultar("usp_ProductosListar").DefaultView; }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Limpiar()
        {
            grid.SelectedItem = null;
            txtId.Clear();
            txtNombre.Clear();
            cboProveedor.SelectedIndex = -1;
            cboCategoria.SelectedIndex = -1;
            txtCantidadUnidad.Clear();
            txtPrecio.Text = "0";
            txtExistencia.Text = "0";
            txtEnPedido.Text = "0";
            txtReorden.Text = "0";
            chkDescontinuado.IsChecked = false;
            txtNombre.Focus();
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Ui.Columnas(e, "ProductoID");
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is DataRowView fila)
            {
                txtId.Text = Ui.Texto(fila["ProductoID"]);
                txtNombre.Text = Ui.Texto(fila["NombreProducto"]);
                cboProveedor.SelectedValue = Ui.Valor(fila["ProveedorID"]);
                cboCategoria.SelectedValue = Ui.Valor(fila["CategoriaID"]);
                txtCantidadUnidad.Text = Ui.Texto(fila["CantidadPorUnidad"]);
                txtPrecio.Text = Convert.ToDecimal(fila["PrecioUnidad"]).ToString("0.00", CultureInfo.InvariantCulture);
                txtExistencia.Text = Ui.Texto(fila["UnidadesEnExistencia"]);
                txtEnPedido.Text = Ui.Texto(fila["UnidadesEnPedido"]);
                txtReorden.Text = Ui.Texto(fila["NivelDeReorden"]);
                chkDescontinuado.IsChecked = Convert.ToBoolean(fila["Descontinuado"]);
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
                Ui.Info("Ingrese el nombre del producto.");
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio) || precio < 0 ||
                !short.TryParse(txtExistencia.Text, out short existencia) ||
                !short.TryParse(txtEnPedido.Text, out short enPedido) ||
                !short.TryParse(txtReorden.Text, out short reorden))
            {
                Ui.Info("Verifique los valores numéricos (use punto como separador decimal).");
                return;
            }
            try
            {
                var parametros = new[]
                {
                    Db.P("@NombreProducto", txtNombre.Text.Trim()),
                    Db.P("@ProveedorID", cboProveedor.SelectedValue),
                    Db.P("@CategoriaID", cboCategoria.SelectedValue),
                    Db.P("@CantidadPorUnidad", txtCantidadUnidad.Text),
                    Db.P("@PrecioUnidad", precio),
                    Db.P("@UnidadesEnExistencia", existencia),
                    Db.P("@UnidadesEnPedido", enPedido),
                    Db.P("@NivelDeReorden", reorden),
                    Db.P("@Descontinuado", chkDescontinuado.IsChecked == true)
                };
                if (string.IsNullOrEmpty(txtId.Text))
                    Db.Ejecutar("usp_ProductoInsertar", parametros);
                else
                {
                    var todos = new Microsoft.Data.SqlClient.SqlParameter[parametros.Length + 1];
                    todos[0] = Db.P("@ProductoID", int.Parse(txtId.Text));
                    parametros.CopyTo(todos, 1);
                    Db.Ejecutar("usp_ProductoActualizar", todos);
                }
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                Ui.Info("Seleccione un producto.");
                return;
            }
            if (!Ui.Confirmar("¿Eliminar el producto seleccionado?")) return;
            try
            {
                Db.Ejecutar("usp_ProductoEliminar", Db.P("@ProductoID", int.Parse(txtId.Text)));
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }
    }
}
