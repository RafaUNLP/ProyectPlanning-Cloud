using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] //exige token jwt para todos los enpoints (salvo los [AllowAnonymous])
public class UsuarioController : ControllerBase{

    private readonly UsuarioRepository _usuarioRepository;
    private readonly AuthService _authService;

    public UsuarioController(UsuarioRepository usuarioRepository, AuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _authService = authService;
    }
    
    /// <summary>
    /// Crea un usuario.
    /// </summary>
    /// <param name="usuarioDTO">Nombre de usuario y contraseña.</param>
    /// <returns>El usuario creado.</returns>
    /// <response code="200">Creación exitosa.</response>
    /// <response code="409">Nombre de usuario en uso.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /Usuario
    ///     {
    ///        "nombre": "walter.bates",
    ///        "contraseña": "bpm"
    ///     }
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///        "nombre": "walter.bates",
    ///        "contraseña": "eyJhbGciOiJIUzI1NiIsInR..."
    ///     }
    /// </remarks>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CrearUsuario(UsuarioDTO usuarioDTO)
    {
        try
        {
            bool yaExiste = await _usuarioRepository.Exist(c => c.Nombre == usuarioDTO.Nombre);

            if (yaExiste)
                return Conflict("El nombre de usuario se encuentra en uso. Por favor, elija otro e intente más tarde.");

            Usuario creada = await _usuarioRepository.AddAsync(new Usuario()
            {
                Nombre = usuarioDTO.Nombre,
                Contraseña = _authService.HashearContraseña(usuarioDTO.Contraseña)
            });

            return Ok(creada);
        }
        catch
        {
            return StatusCode(500, "Falló la creación del usuario");
        }
    }

    /// <summary>
    /// Recupera un usuario.
    /// </summary>
    /// <param name="nombre">Nombre de usuario.</param>
    /// <returns>El usuario recuperado.</returns>
    /// <response code="200">Recuperación exitosa.</response>
    /// <response code="404">Usuario no encontrado.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /Usuario/walter.bates
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///        "nombre": "walter.bates",
    ///        "contraseña": "eyJhbGciOiJIUzI1NiIsInR..."
    ///     }
    /// </remarks>
    [HttpGet("{nombre}")]
    public async Task<IActionResult> RecuperarUsuario(string nombre)
    {
        try
        {
            Usuario? buscado = await _usuarioRepository.GetAsync(nombre);

            if (buscado == null)
                return NotFound();

            return Ok(buscado);
        }
        catch
        {
            return StatusCode(500, "Falló la recuperación del usuario");
        }
    }

}