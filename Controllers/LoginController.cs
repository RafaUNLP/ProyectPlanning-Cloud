using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Repositories;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase{

    private readonly AuthService _authService;

    public LoginController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Inicia sesión de usuario y devuelve un token JWT.
    /// </summary>
    /// <param name="usuarioDTO">Nombre de usuario y contraseña.</param>
    /// <returns>Token JWT si las credenciales son válidas.</returns>
    /// <response code="200">Login exitoso.</response>
    /// <response code="401">Credenciales inválidas.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /Login
    ///     {
    ///        "nombre": "walter.bates",
    ///        "contraseña": "bpm"
    ///     }
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///        "token": "eyJhbGciOiJIUzI1NiIsInR..."
    ///     }
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Login(UsuarioDTO usuarioDTO)
    {
        try
        {
            string? token = await _authService.Login(usuarioDTO.Nombre,usuarioDTO.Contraseña);

            if (token == null)
                return Unauthorized("Credenciales inválidas.");
            else
                return Ok(token);
        }
        catch
        {
            return StatusCode(500, "Falló el login");
        }
    }
}