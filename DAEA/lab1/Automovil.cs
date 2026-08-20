using System;

namespace Lab1;

/// <summary>
/// Clase derivada de Vehiculo. Puntos 3, 4 y 8 del laboratorio.
/// </summary>
public class Automovil : Vehiculo
{
    // Punto 3: propiedad adicional.
    private string combustible;

    public string Combustible { get { return combustible; } }

    // Punto 4: constructor con todas las propiedades, incluidas las heredadas.
    public Automovil(string marca, string modelo, int año, string combustible)
        : base(marca, modelo, año)
    {
        this.combustible = combustible;
    }

    // Punto 8: sobrescribe el metodo y agrega su informacion especifica.
    public override void MostrarInformacion()
    {
        Console.WriteLine("Tipo   : Automovil");
        base.MostrarInformacion();
        Console.WriteLine("Combustible : " + combustible);
    }
}
