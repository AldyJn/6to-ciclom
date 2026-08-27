using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using lab2.Models;

namespace lab2.Data
{
    public static class Datos
    {
        public static ObservableCollection<Conductor> Conductores { get; } = new ObservableCollection<Conductor>
        {
            new Conductor { Nombre = "Luis Ramos", Licencia = "Q12345678", Transporte = "Transportes Andina" },
            new Conductor { Nombre = "Marco Diaz", Licencia = "Q87654321", Transporte = "Cargo Sur" }
        };

        public static ObservableCollection<Transportista> Transportistas { get; } = new ObservableCollection<Transportista>
        {
            new Transportista { Nombre = "Transportes Andina", Ruc = "20145879632", Telefono = "985412367" },
            new Transportista { Nombre = "Cargo Sur", Ruc = "20458796321", Telefono = "974125896" }
        };

        public static ObservableCollection<Camion> Camiones { get; } = new ObservableCollection<Camion>
        {
            new Camion { Placa = "ABC-123", Marca = "Volvo", Transporte = "Transportes Andina" },
            new Camion { Placa = "XYZ-987", Marca = "Scania", Transporte = "Cargo Sur" }
        };

        public static ObservableCollection<Producto> Productos { get; } = new ObservableCollection<Producto>
        {
            new Producto { Nombre = "Maiz", Unidad = "TN" },
            new Producto { Nombre = "Trigo", Unidad = "TN" },
            new Producto { Nombre = "Soya", Unidad = "TN" }
        };

        public static ObservableCollection<Ingreso> Ingresos { get; } = new ObservableCollection<Ingreso>
        {
            new Ingreso { TipoDocumento = "DNI", NumeroDocumento = "72584136", Placa = "ABC-123", Turno = "Mañana", Conductor = "Luis Ramos", Cliente = "Agroindustrias SAC", Producto = "Maiz", Transporte = "Transportes Andina", Fecha = DateTime.Today.AddDays(-2).AddHours(8), Peso = 32500 },
            new Ingreso { TipoDocumento = "DNI", NumeroDocumento = "45896321", Placa = "XYZ-987", Turno = "Tarde", Conductor = "Marco Diaz", Cliente = "Molinos del Norte", Producto = "Trigo", Transporte = "Cargo Sur", Fecha = DateTime.Today.AddDays(-1).AddHours(15), Peso = 28900 }
        };

        public static ObservableCollection<Salida> Salidas { get; } = new ObservableCollection<Salida>
        {
            new Salida { NumeroDocumento = "72584136", Placa = "ABC-123", Turno = "Mañana", Conductor = "Luis Ramos", Producto = "Maiz", Fecha = DateTime.Today.AddDays(-2).AddHours(11), Peso = 14200 },
            new Salida { NumeroDocumento = "45896321", Placa = "XYZ-987", Turno = "Tarde", Conductor = "Marco Diaz", Producto = "Trigo", Fecha = DateTime.Today.AddDays(-1).AddHours(18), Peso = 12750 }
        };

        public static string[] TiposDocumento { get; } = { "DNI", "Carné de extranjería", "Pasaporte" };

        public static string[] Turnos { get; } = { "Mañana", "Tarde", "Noche" };

        public static List<Carga> ObtenerCargas()
        {
            return Ingresos.Select(i =>
            {
                var s = Salidas.FirstOrDefault(x => x.Placa == i.Placa && x.NumeroDocumento == i.NumeroDocumento);
                decimal salida = s == null ? 0 : s.Peso;
                return new Carga
                {
                    Placa = i.Placa,
                    Conductor = i.Conductor,
                    Producto = i.Producto,
                    Fecha = i.Fecha,
                    PesoIngreso = i.Peso,
                    PesoSalida = salida,
                    PesoNeto = i.Peso - salida
                };
            }).ToList();
        }
    }
}
