namespace AuthService.API.DTOs;

public record LoginRequest(string NombreUsuario, string Password);
public record RegisterRequest(string NombreUsuario, string Email, string Password);
public record AuthResponse(string Token);