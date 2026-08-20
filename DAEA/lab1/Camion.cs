using System;

namespace Lab1;

/// <summary>
/// Clase derivada de Vehiculo. Puntos 5, 6 y 8 del laboratorio.
/// </summary>
public class Camion : Vehiculo
{
    /// <summary>Recargo en soles por cada tonelada transportada.</summary>
    private const double TarifaPorTonelada = 15.0;

    // Punto 5: propiedad adicional (en toneladas).
    private double capacidadCarga;

    public double CapacidadCarga { get { return capacidadCarga; } }

    // Punto 6: constructor con todas las propiedades, incluidas las heredadas.
    public Camion(string marca, string modelo, int año, double capacidadCarga)
        : base(marca, modelo, año)
    {
        this.capacidadCarga = capacidadCarga;
    }

    // Punto 8: sobrescribe el metodo y agrega su informacion especifica.
    public override void MostrarInformacion()
    {
        Console.WriteLine("Tipo   : Camion");
        base.MostrarInformacion();
        Console.WriteLine("Capacidad de carga : " + capacidadCarga.ToString("N2") + " toneladas");
    }

    /// <summary>
    /// Al costo de combustible se le suma un recargo proporcional a la carga.
    /// </summary>
    public override double CalcularCostoViaje(double distanciaKm, double rendimientoKmPorGalon, double precioCombustible)
    {
        double costoCombustible = base.CalcularCostoViaje(distanciaKm, rendimientoKmPorGalon, precioCombustible);
        return costoCombustible + (capacidadCarga * TarifaPorTonelada);
    }
}
