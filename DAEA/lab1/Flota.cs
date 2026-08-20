using System;
using System.Collections.Generic;

namespace Lab1;

/// <summary>
/// Punto 9: contiene una lista de vehiculos y los recorre polimorficamente.
/// </summary>
public class Flota
{
    private List<Vehiculo> vehiculos;

    public Flota()
    {
        vehiculos = new List<Vehiculo>();
    }

    public int Cantidad { get { return vehiculos.Count; } }

    public void AgregarVehiculo(Vehiculo vehiculo)
    {
        if (vehiculo == null)
            throw new ArgumentNullException("vehiculo");

        vehiculos.Add(vehiculo);
    }

    /// <summary>
    /// Recorre la lista y llama a MostrarInformacion de cada vehiculo.
    /// Como el metodo es virtual, se ejecuta la version de la clase real.
    /// </summary>
    public void MostrarFlota()
    {
        if (vehiculos.Count == 0)
        {
            Console.WriteLine("La flota no tiene vehiculos registrados.");
            return;
        }

        foreach (Vehiculo vehiculo in vehiculos)
        {
            vehiculo.MostrarInformacion();
            Console.WriteLine("------------------------------");
        }
    }
}
