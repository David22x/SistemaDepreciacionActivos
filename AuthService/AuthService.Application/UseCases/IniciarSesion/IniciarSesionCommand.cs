namespace AuthService.Application.UseCases.IniciarSesion;

public record IniciarSesionCommand(string NombreUsuario, string Password);