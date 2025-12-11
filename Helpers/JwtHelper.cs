using Microsoft.Extensions.DependencyInjection;

namespace SistemaGestionAgricola.Helpers
{
    public static class JwtHelper
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var secretKey = jwtSettings.GetValue<string>("SecretKey") ?? throw new ArgumentNullException("Jwt:SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? throw new ArgumentNullException("Jwt:Issuer");
            var audience = jwtSettings.GetValue<string>("Audience") ?? throw new ArgumentNullException("Jwt:Audience");
            
            // Configuración de JWT aquí
            // services.AddAuthentication...
            
            return services;
        }
    }
}