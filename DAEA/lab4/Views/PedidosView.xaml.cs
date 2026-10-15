using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using lab4.Data;

namespace lab4.Views
{
    public partial class PedidosView : UserControl
    {
        public PedidosView()
        {
            InitializeComponent();
            try
            {
                Ui.Combo(cboCliente, "usp_ClientesCombo");
                Ui.Combo(cboEmpleado, "usp_EmpleadosCombo");
                Ui.Combo(cboTransportista, "usp_TransportistasCombo");
            }
            catch (Exception ex) { Ui.Error(ex); }
            dpPedido.SelectedDate = DateTime.Today;
            dpDesde.SelectedDate = new DateTime(2026, 8, 1);
            dpHasta.SelectedDate = new DateTime(2026, 8, 31);
            Cargar();
        }

        private void Cargar()
        {
            try { grid.ItemsSource = Db.Consultar("usp_PedidosListar").DefaultView; }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Limpiar()
        {
            grid.SelectedItem = null;
            txtId.Clear();
            cboCliente.SelectedIndex = -1;
            cboEmpleado.SelectedIndex = -1;
            cboTransportista.SelectedIndex = -1;
            dpPedido.SelectedDate = DateTime.Today;
            dpRequerida.SelectedDate = null;
            dpEnvio.SelectedDate = null;
            txtDestinatario.Clear();
            txtCiudad.Clear();
            txtPais.Clear();
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Ui.Columnas(e, "PedidoID");
        }

        private void GridReporte_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Ui.Columnas(e, "PedidoID");
        }

        private static DateTime? Fecha(object celda)
        {
            return celda == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(celda);
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is DataRowView fila)
            {
                txtId.Text = Ui.Texto(fila["PedidoID"]);
                cboCliente.SelectedValue = Ui.Valor(fila["ClienteID"]);
                cboEmpleado.SelectedValue = Ui.Valor(fila["EmpleadoID"]);
                cboTransportista.SelectedValue = Ui.Valor(fila["TransportistaID"]);
                dpPedido.SelectedDate = Fecha(fila["FechaPedido"]);
                dpRequerida.SelectedDate = Fecha(fila["FechaRequerida"]);
                dpEnvio.SelectedDate = Fecha(fila["FechaEnvio"]);
                txtDestinatario.Text = Ui.Texto(fila["Destinatario"]);
                txtCiudad.Text = Ui.Texto(fila["CiudadDestino"]);
                txtPais.Text = Ui.Texto(fila["PaisDestino"]);
            }
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (dpPedido.SelectedDate == null)
            {
                Ui.Info("Seleccione la fecha del pedido.");
                return;
            }
            try
            {
                var datos = new[]
                {
                    Db.P("@ClienteID", cboCliente.SelectedValue),
                    Db.P("@EmpleadoID", cboEmpleado.SelectedValue),
                    Db.P("@FechaPedido", dpPedido.SelectedDate),
                    Db.P("@FechaRequerida", dpRequerida.SelectedDate),
                    Db.P("@FechaEnvio", dpEnvio.SelectedDate),
                    Db.P("@TransportistaID", cboTransportista.SelectedValue),
                    Db.P("@Destinatario", txtDestinatario.Text),
                    Db.P("@CiudadDestino", txtCiudad.Text),
                    Db.P("@PaisDestino", txtPais.Text)
                };
                if (string.IsNullOrEmpty(txtId.Text))
                    Db.Ejecutar("usp_PedidoInsertar", datos);
                else
                {
                    var todos = new Microsoft.Data.SqlClient.SqlParameter[datos.Length + 1];
                    todos[0] = Db.P("@PedidoID", int.Parse(txtId.Text));
                    datos.CopyTo(todos, 1);
                    Db.Ejecutar("usp_PedidoActualizar", todos);
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
                Ui.Info("Seleccione un pedido.");
                return;
            }
            if (!Ui.Confirmar("¿Eliminar el pedido seleccionado junto con su detalle?")) return;
            try
            {
                Db.Ejecutar("usp_PedidoEliminar", Db.P("@PedidoID", int.Parse(txtId.Text)));
                Cargar();
                Limpiar();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        private void Reporte_Click(object sender, RoutedEventArgs e)
        {
            if (dpDesde.SelectedDate == null || dpHasta.SelectedDate == null)
            {
                Ui.Info("Seleccione el intervalo de fechas.");
                return;
            }
            if (dpDesde.SelectedDate > dpHasta.SelectedDate)
            {
                Ui.Info("La fecha inicial no puede ser mayor que la final.");
                return;
            }
            try
            {
                var tabla = Db.Consultar("usp_DetallePedidosPorFechas",
                    Db.P("@FechaDesde", dpDesde.SelectedDate),
                    Db.P("@FechaHasta", dpHasta.SelectedDate));
                gridReporte.ItemsSource = tabla.DefaultView;
                decimal total = 0;
                foreach (DataRow fila in tabla.Rows) total += Convert.ToDecimal(fila["Subtotal"]);
                lblTotal.Text = $"Registros: {tabla.Rows.Count}    Total: S/ {total:N2}";
            }
            catch (Exception ex) { Ui.Error(ex); }
        }
    }
}
