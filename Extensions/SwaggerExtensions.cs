using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SistemaGestionAgricola.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Sistema Gestión Agrícola API", 
                    Version = "v1",
                    Description = "API RESTful para la gestión agrícola integral"
                });

                // Configurar autenticación JWT en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header - utiliza: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}