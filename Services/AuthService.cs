using backend.Models;
using backend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly UsuarioRepository _usuarioRepository;
    public AuthService(IConfiguration configuration, UsuarioRepository usuarioRepository)
    {
        _configuration = configuration;
        _usuarioRepository = usuarioRepository;
    }
    //traje del controller de Auth solamente aquello que se quiere reutilizar
    public string HashearContraseña(string contraseña)
    {
        byte[] data = Encoding.UTF8.GetBytes(contraseña); // Cambié a UTF8 para evitar problemas con caracteres fuera de ASCII
        data = System.Security.Cryptography.SHA256.HashData(data);
        string hash = Convert.ToBase64String(data); // Convertimos el hash a Base64
        return hash;
    }
    
    private string GenerarTokenAuth(string nombreUsuario)
    {
        var clave = _configuration["ConfigurationJwt:SecretKey"];
        if (string.IsNullOrEmpty(clave))
            throw new InvalidOperationException("No se encontró ConfigurationJwt:SecretKey en la configuración");

        var issuer = _configuration["ConfigurationJwt:Issuer"];
        if (string.IsNullOrEmpty(issuer))
            throw new InvalidOperationException("No se encontró ConfigurationJwt:Issuer en la configuración");

        var claveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
        var credenciales = new SigningCredentials(claveSeguridad, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, nombreUsuario)
        };

        DateTime fechaExpiracion = DateTime.Now.AddHours(3); //3hs de duración

        var secToken = new JwtSecurityToken(
            issuer: issuer,
            audience: null,
            claims: claims,
            expires: fechaExpiracion,
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(secToken);
    }

    public async Task<string?> Login(string nombre, string contraseña)
    {

        Usuario? usuario = (await _usuarioRepository.FilterAsync(x => x.Nombre == nombre)).FirstOrDefault();

        if (usuario == null)
            return null;

        if (usuario.Contraseña == this.HashearContraseña(contraseña))
            return this.GenerarTokenAuth(usuario.Nombre);;

        return null;
    }
}
