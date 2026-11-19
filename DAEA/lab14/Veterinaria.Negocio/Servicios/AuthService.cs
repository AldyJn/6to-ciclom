using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Seguridad;

namespace Veterinaria.Negocio.Servicios;

public class AuthService : IAuthService
{
    private const string RolPorDefecto = Roles.Asistente;

    private readonly IUsuarioRepository _usuarios;
    private readonly IMapper _mapper;
    private readonly JwtOpciones _jwt;

    public AuthService(IUsuarioRepository usuarios, IMapper mapper, JwtOpciones jwt)
    {
        _usuarios = usuarios;
        _mapper = mapper;
        _jwt = jwt;
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = NormalizarCorreo(dto.Correo);

        if (await _usuarios.ObtenerPorCorreoAsync(correo) != null)
            throw new CorreoDuplicadoException(correo);

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolPorDefecto,
            FechaRegistro = DateTime.Now
        };

        await _usuarios.AgregarAsync(usuario);
        await _usuarios.GuardarAsync();

        return _mapper.Map<UsuarioDto>(usuario);
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarios.ObtenerPorCorreoAsync(NormalizarCorreo(dto.Correo));

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var expiracion = DateTime.UtcNow.AddMinutes(_jwt.MinutosExpiracion);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new("role", usuario.Rol),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Clave));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Emisor,
            audience: _jwt.Audiencia,
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }

    private static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
}
