using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SistemaGestionAgricola.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var secretKey = jwtSection.GetValue<string>("SecretKey") ?? 
                           throw new ArgumentNullException("Jwt:SecretKey", "JWT SecretKey no configurada");
            var issuer = jwtSection.GetValue<string>("Issuer") ?? 
                        throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer no configurado");
            var audience = jwtSection.GetValue<string>("Audience") ?? 
                          throw new ArgumentNullException("Jwt:Audience", "JWT Audience no configurado");

            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }
    }
}