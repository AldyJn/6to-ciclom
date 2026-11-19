namespace Veterinaria.Negocio.Seguridad;

public class JwtOpciones
{
    public const string Seccion = "Jwt";

    public string Clave { get; set; } = string.Empty;

    public string Emisor { get; set; } = string.Empty;

    public string Audiencia { get; set; } = string.Empty;

    public int MinutosExpiracion { get; set; } = 60;
}
