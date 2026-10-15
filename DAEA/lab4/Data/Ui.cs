using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace lab4.Data
{
    public static class Ui
    {
        public static void Error(Exception ex)
        {
            string mensaje = ex.Message;
            if (ex is SqlException sql && sql.Number == 547)
                mensaje = "No se puede completar la operación porque el registro está relacionado con otros datos.";
            MessageBox.Show(mensaje, "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void Info(string mensaje)
        {
            MessageBox.Show(mensaje, "NeptunoDB", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool Confirmar(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question)
                   == MessageBoxResult.Yes;
        }

        public static void Columnas(DataGridAutoGeneratingColumnEventArgs e, string clavePrimaria)
        {
            if (e.PropertyName.EndsWith("ID") && e.PropertyName != clavePrimaria)
                e.Cancel = true;
            else if (e.Column is DataGridTextColumn col && e.PropertyType == typeof(decimal))
                col.Binding.StringFormat = "N2";
            else if (e.PropertyType == typeof(DateTime))
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "dd/MM/yyyy";
        }

        public static void Combo(ComboBox combo, string procedimiento)
        {
            combo.ItemsSource = Db.Consultar(procedimiento).DefaultView;
            combo.DisplayMemberPath = "Nombre";
            combo.SelectedValuePath = "Id";
        }

        public static object Valor(object celda)
        {
            return celda == DBNull.Value ? null : celda;
        }

        public static string Texto(object celda)
        {
            return celda == DBNull.Value ? "" : Convert.ToString(celda);
        }
    }
}
