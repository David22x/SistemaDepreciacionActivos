using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario);
}