using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SistemaGestionAgricola.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "EstaEsUnaClaveSecretaSuperLargaDe64CaracteresParaJWTEnSistemaAgricola2025!@#";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SistemaGestionAgricola";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SistemaGestionAgricolaClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        
        // Para debugging (opcional)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Add DbContext - USANDO MySQL (Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada");
}

// Asegúrate de tener: dotnet add package Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));

// Servicios de aplicación
builder.Services.AddScoped<IJwtService, JwtService>();
// Agrega MemoryCache
builder.Services.AddMemoryCache();

// ⭐⭐⭐⭐ CONFIGURACIÓN CRÍTICA DE EMAIL ⭐⭐⭐⭐
// Esto debe ser EmailConfiguration (NO EmailSettings)
builder.Services.Configure<EmailConfiguration>(  // ← CAMBIA A EmailConfiguration
    builder.Configuration.GetSection("EmailSettings"));

// Servicios de Email
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordValidator, PasswordValidator>(); // Contraseña Segura

// Configurar CORS si necesitas frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000", "http://localhost:5173") // Tu frontend
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Agregar logging mejorado para emails
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Information);
});

// Add Swagger CON AUTORIZACIÓN JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Sistema Gestión Agrícola API", 
        Version = "v1",
        Description = "API para el sistema de gestión agrícola",
        Contact = new OpenApiContact
        {
            Name = "Soporte",
            Email = "soporte@sistemaagricola.com"
        }
    });
    
    // CONFIGURACIÓN DE AUTORIZACIÓN JWT EN SWAGGER
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT. Ejemplo: Bearer eyJhbGciOi..."
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
            Array.Empty<string>()
        }
    });
    
    // Esto ayuda a mostrar mejor los enums
    c.UseAllOfToExtendReferenceSchemas();
    
    // Opcional: Ordenar los endpoints por nombre
    c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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
    using (var scope = app.Services.CreateScope())
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
app.Use(async (context, next) =>
{
    // Solo para endpoints de autenticación
    if (context.Request.Path.StartsWithSegments("/api/Auth"))
    {
        await next();
        return;
    }
    
    Console.WriteLine($"\n=== REQUEST {DateTime.Now:HH:mm:ss} ===");
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
    
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    if (string.IsNullOrEmpty(authHeader))
    {
        Console.WriteLine("⚠️ NO hay Authorization header");
    }
    else
    {
        Console.WriteLine($"Authorization: {authHeader}");
        
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            Console.WriteLine($"Token recibido: {token.Substring(0, Math.Min(30, token.Length))}...");
            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.CanReadToken(token))
                {
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    Console.WriteLine($"✅ Token válido - Claims:");
                    foreach (var claim in jwtToken.Claims)
                    {
                        Console.WriteLine($"  {claim.Type}: {claim.Value}");
                    }
                    Console.WriteLine($"Expira: {jwtToken.ValidTo.ToLocalTime()}");
                }
                else
                {
                    Console.WriteLine("❌ Token NO puede leerse");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error leyendo token: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("❌ Header no empieza con 'Bearer'");
        }
    }
    
    await next();
    
    Console.WriteLine($"Response Status: {context.Response.StatusCode}");
    Console.WriteLine("=== END REQUEST ===\n");
});

Console.WriteLine("🚀 Aplicación iniciada en: " + (app.Environment.IsDevelopment() ? "http://localhost:5173" : "Producción"));
Console.WriteLine("📚 Swagger disponible en: http://localhost:5173");
Console.WriteLine("🔐 Recuerda usar el botón 'Authorize' en Swagger para probar endpoints protegidos");

app.Run();