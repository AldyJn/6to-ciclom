using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Neptuno.Datos.Modelos;
using Neptuno.Datos.Repositorios;

namespace Neptuno.App.Vistas
{
    public partial class PedidosView : UserControl
    {
        private readonly PedidoRepositorio repositorio = new PedidoRepositorio();
        private readonly CatalogoRepositorio catalogos = new CatalogoRepositorio();
        private int pedidoId;

        public PedidosView()
        {
            InitializeComponent();
            Loaded += PedidosView_Loaded;
        }

        private async void PedidosView_Loaded(object sender, RoutedEventArgs e)
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
            if (dpFechaPedido.SelectedDate == null)
            {
                MessageBox.Show("La fecha del pedido es obligatoria.");
                return;
            }

            var pedido = new Pedido
            {
                PedidoID = pedidoId,
                ClienteID = (int?)cboCliente.SelectedValue,
                EmpleadoID = (int?)cboEmpleado.SelectedValue,
                TransportistaID = (int?)cboTransportista.SelectedValue,
                FechaPedido = dpFechaPedido.SelectedDate.Value,
                FechaRequerida = dpFechaRequerida.SelectedDate,
                FechaEnvio = dpFechaEnvio.SelectedDate,
                Destinatario = txtDestinatario.Text,
                CiudadDestino = txtCiudadDestino.Text
            };

            try
            {
                if (pedidoId == 0)
                    await repositorio.InsertarAsync(pedido);
                else
                    await repositorio.ActualizarAsync(pedido);

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
            if (pedidoId == 0)
            {
                MessageBox.Show("Seleccione un pedido.");
                return;
            }

            var respuesta = MessageBox.Show(
                "El pedido quedara con Activo = 0 y saldra del reporte. Continuar?",
                "Eliminacion logica", MessageBoxButton.YesNo);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                await repositorio.EliminarAsync(pedidoId);
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

            pedidoId = Convert.ToInt32(fila["PedidoID"]);
            cboCliente.SelectedValue = fila["ClienteID"] == DBNull.Value ? null : fila["ClienteID"];
            cboEmpleado.SelectedValue = fila["EmpleadoID"] == DBNull.Value ? null : fila["EmpleadoID"];
            cboTransportista.SelectedValue = fila["TransportistaID"] == DBNull.Value ? null : fila["TransportistaID"];
            dpFechaPedido.SelectedDate = Convert.ToDateTime(fila["FechaPedido"]);
            dpFechaRequerida.SelectedDate = Fecha(fila["FechaRequerida"]);
            dpFechaEnvio.SelectedDate = Fecha(fila["FechaEnvio"]);
            txtDestinatario.Text = fila["Destinatario"].ToString();
            txtCiudadDestino.Text = fila["CiudadDestino"].ToString();
            lblEstado.Text = "Editando el pedido " + pedidoId;
        }

        private async Task CargarAsync()
        {
            try
            {
                var tabla = await repositorio.ListarAsync();
                grilla.ItemsSource = tabla.DefaultView;
                lblEstado.Text = tabla.Rows.Count + " pedidos activos";
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
                cboCliente.ItemsSource = (await catalogos.ClientesAsync()).DefaultView;
                cboEmpleado.ItemsSource = (await catalogos.EmpleadosAsync()).DefaultView;
                cboTransportista.ItemsSource = (await catalogos.TransportistasAsync()).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static DateTime? Fecha(object valor)
            => valor == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(valor);

        private void Limpiar()
        {
            pedidoId = 0;
            cboCliente.SelectedIndex = -1;
            cboEmpleado.SelectedIndex = -1;
            cboTransportista.SelectedIndex = -1;
            dpFechaPedido.SelectedDate = DateTime.Today;
            dpFechaRequerida.SelectedDate = null;
            dpFechaEnvio.SelectedDate = null;
            txtDestinatario.Clear();
            txtCiudadDestino.Clear();
            lblEstado.Text = "Nuevo pedido";
        }
    }
}
