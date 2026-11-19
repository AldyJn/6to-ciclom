namespace Veterinaria.Entidades.Dtos;

public class TokenDto
{
    public string Token { get; set; } = null!;

    public DateTime Expiracion { get; set; }
}
