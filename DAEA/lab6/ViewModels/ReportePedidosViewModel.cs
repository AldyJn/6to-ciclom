using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using lab6.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.ViewModels
{
    /// <summary>
    /// Punto 12.a: reporte del detalle de pedidos filtrado por un intervalo de
    /// fechas. Se apoya en usp_DetallePedidosPorFechas, que hace inner join con
    /// Pedidos y descarta los que tienen Activo = 0.
    /// </summary>
    public class ReportePedidosViewModel : ViewModelBase
    {
        private readonly PedidoRepositorio _repositorio = new PedidoRepositorio();
        private DateTime? _desde = new DateTime(DateTime.Today.Year, 1, 1);
        private DateTime? _hasta = new DateTime(DateTime.Today.Year, 12, 31);
        private string _mensaje;
        private decimal _total;

        public ReportePedidosViewModel()
        {
            Detalles = new ObservableCollection<DetallePedido>();
            GenerarCommand = new Comando(Generar);

            Generar();
        }

        public ObservableCollection<DetallePedido> Detalles { get; }

        public DateTime? Desde
        {
            get => _desde;
            set { _desde = value; Notificar(); }
        }

        public DateTime? Hasta
        {
            get => _hasta;
            set { _hasta = value; Notificar(); }
        }

        public decimal Total
        {
            get => _total;
            set { _total = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public ICommand GenerarCommand { get; }

        private void Generar()
        {
            if (!Desde.HasValue || !Hasta.HasValue)
            {
                Mensaje = "Indique la fecha inicial y la fecha final";
                return;
            }

            if (Hasta < Desde)
            {
                Mensaje = "La fecha final no puede ser anterior a la fecha inicial";
                return;
            }

            try
            {
                Detalles.Clear();
                foreach (var detalle in _repositorio.DetallesPorFechas(Desde.Value, Hasta.Value))
                    Detalles.Add(detalle);

                Total = Detalles.Sum(d => d.Subtotal);

                Mensaje = Detalles.Count == 0
                    ? "No hay pedidos activos en el intervalo seleccionado"
                    : Contar(Detalles.Count, "linea de detalle", "lineas de detalle") + " entre " +
                      Desde.Value.ToString("dd/MM/yyyy") + " y " + Hasta.Value.ToString("dd/MM/yyyy");
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al generar el reporte: " + ex.Message;
            }
        }
    }
}
