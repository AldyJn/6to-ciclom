using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IAuthService
{
    Task<UsuarioDto> RegistrarAsync(RegistroDto dto);

    Task<TokenDto?> LoginAsync(LoginDto dto);
}
