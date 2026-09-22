using AuthService.API.DTOs;
using AuthService.Application.UseCases.IniciarSesion;
using AuthService.Application.UseCases.RegistrarUsuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IniciarSesionHandler _iniciarSesion;
    private readonly RegistrarUsuarioHandler _registrar;

    public AuthController(
        IniciarSesionHandler iniciarSesion,
        RegistrarUsuarioHandler registrar)
    {
        _iniciarSesion = iniciarSesion;
        _registrar = registrar;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _iniciarSesion.HandleAsync(
            new IniciarSesionCommand(request.NombreUsuario, request.Password));

        if (token is null)
            return Unauthorized(new { mensaje = "Credenciales inválidas" });

        return Ok(new AuthResponse(token));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var usuario = await _registrar.HandleAsync(
                new RegistrarUsuarioCommand(request.NombreUsuario, request.Email, request.Password));
            return CreatedAtAction(nameof(Login), new { id = usuario.Id });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpGet("validar")]
    [Authorize]
    public IActionResult Validar() =>
        Ok(new { mensaje = "Token válido", usuario = User.Identity?.Name });
}