using Microsoft.Extensions.DependencyInjection;

namespace SistemaGestionAgricola.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}