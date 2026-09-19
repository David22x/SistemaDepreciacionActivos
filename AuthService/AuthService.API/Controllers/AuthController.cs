using AuthService.API.DTOs;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService.Infrastructure.Services.AuthService _auth;

    public AuthController(AuthService.Infrastructure.Services.AuthService auth)
        => _auth = auth;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _auth.LoginAsync(request.NombreUsuario, request.Password);
        if (token is null)
            return Unauthorized(new { mensaje = "Credenciales inválidas :(" });

        return Ok(new AuthResponse(token));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var usuario = await _auth.RegistrarAsync(
            request.NombreUsuario, request.Email, request.Password);

        return CreatedAtAction(nameof(Login), new { id = usuario.Id });
    }

    [HttpGet("validar")]
    [Authorize]
    public IActionResult Validar() =>
        Ok(new { mensaje = "Token válido", usuario = User.Identity?.Name });
}