namespace AuthService.Application.UseCases.RegistrarUsuario;

public record RegistrarUsuarioCommand(string NombreUsuario, string Email, string Password);