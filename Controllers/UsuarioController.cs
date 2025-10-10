using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs;
using backend.Repositories;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.HttpResults;
using ApiACEAPP.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] //exige token jwt para todos los enpoints (salvo los [AllowAnonymous])
public class UsuarioController : ControllerBase{

    private readonly UsuarioRepository _usuarioRepository;
    private readonly AuthService _authService;

    public UsuarioController(UsuarioRepository usuarioRepository, AuthService authService) {
        _usuarioRepository = usuarioRepository;
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CrearUsuario(UsuarioDTO usuarioDTO)
    {
        try
        {
            bool yaExiste = await _usuarioRepository.Exist(c => c.Nombre == usuarioDTO.Nombre);

            if (yaExiste)
                return Conflict("El usuario ya existe");

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

    [HttpGet("{nombre}")] //hace falta?
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