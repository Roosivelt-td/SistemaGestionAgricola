
using SistemaGestionAgricola.Models.Entities;
using System.Security.Claims;

namespace SistemaGestionAgricola.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}