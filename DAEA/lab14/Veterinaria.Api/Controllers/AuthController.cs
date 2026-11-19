using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("registro")]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto dto)
    {
        try
        {
            var usuario = await _auth.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, usuario);
        }
        catch (CorreoDuplicadoException ex)
        {
            return Conflict(new ProblemDetails { Status = 409, Title = "Regla de negocio incumplida", Detail = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);

        if (token == null)
            return Unauthorized(new ProblemDetails { Status = 401, Title = "Credenciales invalidas" });

        return Ok(token);
    }

    [HttpGet("perfil")]
    [Authorize]
    public ActionResult<PerfilDto> Perfil()
    {
        var perfil = new PerfilDto
        {
            Correo = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty,
            Rol = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? string.Empty
        };

        return Ok(perfil);
    }
}
