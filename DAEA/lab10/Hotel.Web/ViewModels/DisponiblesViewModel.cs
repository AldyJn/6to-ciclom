namespace Hotel.Web.ViewModels;

public class DisponibleItem
{
    public int IdHabitacion { get; set; }
    public string Numero { get; set; } = "";
    public int Piso { get; set; }
    public string Tipo { get; set; } = "";
    public int Capacidad { get; set; }
    public decimal PrecioNoche { get; set; }
    public int Noches { get; set; }
    public decimal Total { get; set; }
}

public class DisponiblesViewModel
{
    public DateOnly? Ingreso { get; set; }
    public DateOnly? Salida { get; set; }
    public bool Buscado { get; set; }
    public List<DisponibleItem> Items { get; set; } = new();
}
