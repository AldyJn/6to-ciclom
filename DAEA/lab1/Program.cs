using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab1;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // ---------------------------------------------------------------
        // Punto 10: crear un Automovil, un Camion, una Flota y mostrarla.
        // ---------------------------------------------------------------
        Automovil automovil = new Automovil("Toyota", "Corolla", 2020, "Gasolina");
        Camion camion = new Camion("Volvo", "FH16", 2018, 25.5);

        Flota flota = new Flota();
        flota.AgregarVehiculo(automovil);
        flota.AgregarVehiculo(camion);

        Console.WriteLine("=== FLOTA REGISTRADA ===");
        flota.MostrarFlota();

        // ---------------------------------------------------------------
        // Punto 11: lista de tipo Vehiculo.
        // Punto 12: agregar un Automovil y un Camion a la lista.
        // Se agregan los mismos objetos de la flota para que el menu trabaje
        // sobre los vehiculos que se acaban de mostrar.
        // ---------------------------------------------------------------
        List<Vehiculo> vehiculos = new List<Vehiculo>();
        vehiculos.Add(automovil);
        vehiculos.Add(camion);

        // ---------------------------------------------------------------
        // Puntos 13, 14 y 15: menu de opciones.
        // ---------------------------------------------------------------
        bool salir = false;

        while (!salir)
        {
            MostrarMenu();
            string opcion = Console.ReadLine();

            if (opcion == null)   // entrada cerrada (por ejemplo, Ctrl+Z)
                break;

            switch (opcion.Trim())
            {
                case "1":
                    MostrarVehiculos(vehiculos);
                    break;

                case "2":
                    CalcularCostoViaje(vehiculos);
                    break;

                case "3":
                    salir = true;
                    Console.WriteLine("\nSaliendo del programa. Hasta luego.");
                    break;

                default:
                    Console.WriteLine("\nOpcion no valida. Elija 1, 2 o 3.");
                    break;
            }
        }
    }

    private static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("========== MENU ==========");
        Console.WriteLine("1. Mostrar informacion de vehiculos");
        Console.WriteLine("2. Calcular costo de viaje");
        Console.WriteLine("3. Salir");
        Console.WriteLine("==========================");
        Console.Write("Seleccione una opcion: ");
    }

    // Punto 14: accion 1 -> mostrar la informacion de todos los vehiculos.
    private static void MostrarVehiculos(List<Vehiculo> vehiculos)
    {
        Console.WriteLine("\n=== INFORMACION DE VEHICULOS ===");

        if (vehiculos.Count == 0)
        {
            Console.WriteLine("No hay vehiculos registrados.");
            return;
        }

        foreach (Vehiculo vehiculo in vehiculos)
        {
            vehiculo.MostrarInformacion();
            Console.WriteLine("------------------------------");
        }
    }

    // Punto 15: accion 2 -> elegir vehiculo, pedir distancia y calcular el costo.
    private static void CalcularCostoViaje(List<Vehiculo> vehiculos)
    {
        Console.WriteLine("\n=== CALCULO DE COSTO DE VIAJE ===");

        if (vehiculos.Count == 0)
        {
            Console.WriteLine("No hay vehiculos registrados.");
            return;
        }

        for (int i = 0; i < vehiculos.Count; i++)
        {
            Vehiculo v = vehiculos[i];
            Console.WriteLine((i + 1) + ". " + v.Marca + " " + v.Modelo + " (" + v.Año + ")");
        }

        int indice = LeerEntero("Seleccione el vehiculo (1 - " + vehiculos.Count + "): ", 1, vehiculos.Count) - 1;
        Vehiculo elegido = vehiculos[indice];

        double distancia = LeerDouble("Distancia del viaje en km: ", 0.0001, double.MaxValue);
        double rendimiento = LeerDouble("Rendimiento del vehiculo en km por galon: ", 0.0001, double.MaxValue);
        double precio = LeerDouble("Precio del combustible por galon (S/): ", 0, double.MaxValue);

        double costo = elegido.CalcularCostoViaje(distancia, rendimiento, precio);
        double galones = distancia / rendimiento;

        Console.WriteLine("\n--- RESULTADO ---");
        elegido.MostrarInformacion();
        Console.WriteLine("Distancia   : " + distancia.ToString("N2") + " km");
        Console.WriteLine("Combustible : " + galones.ToString("N2") + " galones");

        Camion camion = elegido as Camion;
        if (camion != null)
        {
            double costoCombustible = galones * precio;
            Console.WriteLine("Costo de combustible : S/ " + costoCombustible.ToString("N2"));
            Console.WriteLine("Recargo por carga    : S/ " + (costo - costoCombustible).ToString("N2"));
        }

        Console.WriteLine("COSTO TOTAL : S/ " + costo.ToString("N2"));
        Console.WriteLine("-----------------");
    }

    private static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine();
            int valor;

            if (int.TryParse(entrada, out valor) && valor >= minimo && valor <= maximo)
                return valor;

            Console.WriteLine("Valor no valido. Ingrese un numero entero entre " + minimo + " y " + maximo + ".");
        }
    }

    private static double LeerDouble(string mensaje, double minimo, double maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine();
            double valor;

            if (TryParseDouble(entrada, out valor) && valor >= minimo && valor <= maximo)
                return valor;

            Console.WriteLine("Valor no valido. Ingrese un numero mayor o igual a " + minimo + ".");
        }
    }

    /// <summary>
    /// Acepta tanto "12.5" como "12,5", sin importar la configuracion regional.
    /// </summary>
    private static bool TryParseDouble(string entrada, out double valor)
    {
        if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.CurrentCulture, out valor))
            return true;

        return double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out valor);
    }
}
