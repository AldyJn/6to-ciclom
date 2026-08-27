using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2.Data;
using lab2.Models;

namespace lab2.Views.Reportes
{
    public partial class ReporteIngresosView : UserControl
    {
        public ReporteIngresosView()
        {
            InitializeComponent();
            Mostrar(Datos.Ingresos.ToList());
        }

        private void Mostrar(List<Ingreso> lista)
        {
            grid.ItemsSource = lista;
            lblResumen.Text = lista.Count + " registro(s)   |   Peso total: " + lista.Sum(i => i.Peso).ToString("N2") + " kg";
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            var lista = Datos.Ingresos.AsEnumerable();

            if (dpInicio.SelectedDate != null)
                lista = lista.Where(i => i.Fecha.Date >= dpInicio.SelectedDate.Value.Date);

            if (dpFin.SelectedDate != null)
                lista = lista.Where(i => i.Fecha.Date <= dpFin.SelectedDate.Value.Date);

            string placa = txtPlaca.Text.Trim();
            if (placa.Length > 0)
                lista = lista.Where(i => i.Placa.IndexOf(placa, StringComparison.OrdinalIgnoreCase) >= 0);

            string conductor = txtConductor.Text.Trim();
            if (conductor.Length > 0)
                lista = lista.Where(i => i.Conductor.IndexOf(conductor, StringComparison.OrdinalIgnoreCase) >= 0);

            string producto = txtProducto.Text.Trim();
            if (producto.Length > 0)
                lista = lista.Where(i => i.Producto.IndexOf(producto, StringComparison.OrdinalIgnoreCase) >= 0);

            Mostrar(lista.ToList());
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            dpInicio.SelectedDate = null;
            dpFin.SelectedDate = null;
            txtPlaca.Clear();
            txtConductor.Clear();
            txtProducto.Clear();
            Mostrar(Datos.Ingresos.ToList());
        }
    }
}
