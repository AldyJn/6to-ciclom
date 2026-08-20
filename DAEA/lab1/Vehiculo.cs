using System;

namespace Lab1;

/// <summary>
/// Clase base de la jerarquia de vehiculos.
/// Puntos 1, 2 y 7 del laboratorio.
/// </summary>
public class Vehiculo
{
    // Punto 1: propiedades privadas (campos privados expuestos como solo lectura).
    private string marca;
    private string modelo;
    private int año;

    public string Marca { get { return marca; } }
    public string Modelo { get { return modelo; } }
    public int Año { get { return año; } }

    // Punto 2: constructor que recibe e inicializa todas las propiedades.
    public Vehiculo(string marca, string modelo, int año)
    {
        this.marca = marca;
        this.modelo = modelo;
        this.año = año;
    }

    // Punto 7: metodo virtual que muestra los datos del vehiculo.
    public virtual void MostrarInformacion()
    {
        Console.WriteLine("Marca  : " + marca);
        Console.WriteLine("Modelo : " + modelo);
        Console.WriteLine("Año    : " + año);
    }

    /// <summary>
    /// Costo base del viaje: se calculan los galones necesarios y se multiplican
    /// por el precio del combustible.
    /// </summary>
    public virtual double CalcularCostoViaje(double distanciaKm, double rendimientoKmPorGalon, double precioCombustible)
    {
        if (distanciaKm <= 0)
            throw new ArgumentException("La distancia debe ser mayor que cero.");
        if (rendimientoKmPorGalon <= 0)
            throw new ArgumentException("El rendimiento debe ser mayor que cero.");
        if (precioCombustible < 0)
            throw new ArgumentException("El precio del combustible no puede ser negativo.");

        double galones = distanciaKm / rendimientoKmPorGalon;
        return galones * precioCombustible;
    }
}
