using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Extensions;
using SistemaGestionAgricola.Services;

namespace SistemaGestionAgricola
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers()
                .AddCustomJsonOptions();

            // Configurar CORS
            services.AddCustomCors();

            // Configurar JWT
            services.AddJwtAuthentication(Configuration);

            // Add DbContext - USANDO MySQL (Pomelo)
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada");
            }

            // Asegúrate de tener: dotnet add package Pomelo.EntityFrameworkCore.MySql
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .EnableSensitiveDataLogging());

            // Configurar servicios de aplicación
            services.AddApplicationServices(Configuration);

            // Agregar logging mejorado para emails
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Information);
            });

            // Add Swagger CON AUTORIZACIÓN JWT
            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                // Servir archivos estáticos (para el JS personalizado)
                app.UseStaticFiles();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Gestión Agrícola API v1");
                    c.RoutePrefix = string.Empty; // Swagger en la raíz: http://localhost:5173/
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();

                    // Configuración adicional para facilitar las pruebas
                    c.DefaultModelsExpandDepth(-1); // Oculta el panel de schemas por defecto
                    c.EnableFilter(); // Habilita filtro de búsqueda
                    c.ShowExtensions();
                    // AGREGAR ESTO para JavaScript personalizado
                    c.InjectJavascript("/swagger/custom.js");
                });

                // Aplicar migraciones automáticamente
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    try
                    {
                        dbContext.Database.Migrate();
                        Console.WriteLine("✅ Base de datos migrada correctamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error al migrar base de datos: {ex.Message}");
                    }
                }
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication(); // IMPORTANTE: Primero Authentication
            app.UseAuthorization();   // IMPORTANTE: Luego Authorization
            app.MapControllers();

            // Middleware para logging de requests (opcional)
            app.UseRequestLogging();
        }
    }
}