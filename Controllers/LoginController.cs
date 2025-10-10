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
public class LoginController : ControllerBase{

    private readonly UsuarioRepository _usuarioRepository;
    private readonly AuthService _authService;

    public LoginController(UsuarioRepository usuarioRepository, AuthService authService) {
        _usuarioRepository = usuarioRepository;
        _authService = authService;
    }

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
        catch (Exception ex)
        {
            return StatusCode(500, "Falló el login");
        }
    }
}