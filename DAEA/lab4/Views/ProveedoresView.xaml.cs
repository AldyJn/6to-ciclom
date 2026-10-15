using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using lab4.Data;

namespace lab4.Views
{
    public partial class ProveedoresView : UserControl
    {
        public ProveedoresView()
        {
            InitializeComponent();
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                grid.ItemsSource = Db.Consultar("usp_ProveedoresBuscar",
                    Db.P("@NombreContacto", txtBuscarContacto.Text.Trim()),
                    Db.P("@Ciudad", txtBuscarCiudad.Text.Trim())).DefaultView;
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Limpiar()
        {
            grid.SelectedItem = null;
            txtId.Clear();
            txtCompania.Clear();
            txtContacto.Clear();
            txtCargo.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtPostal.Clear();
            txtPais.Clear();
            txtTelefono.Clear();
            txtFax.Clear();
            txtCompania.Focus();
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Ui.Columnas(e, "ProveedorID");
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is DataRowView fila)
            {
                txtId.Text = Ui.Texto(fila["ProveedorID"]);
                txtCompania.Text = Ui.Texto(fila["CompaniaNombre"]);
                txtContacto.Text = Ui.Texto(fila["NombreContacto"]);
                txtCargo.Text = Ui.Texto(fila["CargoContacto"]);
                txtDireccion.Text = Ui.Texto(fila["Direccion"]);
                txtCiudad.Text = Ui.Texto(fila["Ciudad"]);
                txtPostal.Text = Ui.Texto(fila["CodigoPostal"]);
                txtPais.Text = Ui.Texto(fila["Pais"]);
                txtTelefono.Text = Ui.Texto(fila["Telefono"]);
                txtFax.Text = Ui.Texto(fila["Fax"]);
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Cargar();
        }

        private void Todos_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarContacto.Clear();
            txtBuscarCiudad.Clear();
            Cargar();
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompania.Text))
            {
                Ui.Info("Ingrese el nombre de la compañía.");
                return;
            }
            try
            {
                var datos = new[]
                {
                    Db.P("@CompaniaNombre", txtCompania.Text.Trim()),
                    Db.P("@NombreContacto", txtContacto.Text),
                    Db.P("@CargoContacto", txtCargo.Text),
                    Db.P("@Direccion", txtDireccion.Text),
                    Db.P("@Ciudad", txtCiudad.Text),
                    Db.P("@CodigoPostal", txtPostal.Text),
                    Db.P("@Pais", txtPais.Text),
                    Db.P("@Telefono", txtTelefono.Text),
                    Db.P("@Fax", txtFax.Text)
                };
                if (string.IsNullOrEmpty(txtId.Text))
                    Db.Ejecutar("usp_ProveedorInsertar", datos);
                else
                {
                    var todos = new Microsoft.Data.SqlClient.SqlParameter[datos.Length + 1];
                    todos[0] = Db.P("@ProveedorID", int.Parse(txtId.Text));
                    datos.CopyTo(todos, 1);
                    Db.Ejecutar("usp_ProveedorActualizar", todos);
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
                Ui.Info("Seleccione un proveedor.");
                return;
            }
            if (!Ui.Confirmar("¿Eliminar el proveedor seleccionado?")) return;
            try
            {
                Db.Ejecutar("usp_ProveedorEliminar", Db.P("@ProveedorID", int.Parse(txtId.Text)));
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }
    }
}
