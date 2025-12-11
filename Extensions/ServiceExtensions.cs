using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SistemaGestionAgricola.Services;

namespace SistemaGestionAgricola.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Servicios de dominio
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAgricultorService, AgricultorService>();
            services.AddScoped<ICultivoService, CultivoService>();
            services.AddScoped<ICosechaService, CosechaService>();
            services.AddScoped<ITerreroService, TerreroService>();
            services.AddScoped<IProcesoAgricolaService, ProcesoAgricolaService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IReporteService, ReporteService>();
            services.AddScoped<IClimaService, ClimaService>();
            services.AddScoped<IPrecioMercadoService, PrecioMercadoService>();
            
            // Otros servicios
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPasswordValidator, PasswordValidator>();
        }
    }
}