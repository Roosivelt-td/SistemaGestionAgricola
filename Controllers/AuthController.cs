using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;
using SistemaGestionAgricola.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly ICodigoVerificacionService _codigoService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            AppDbContext context,
            IJwtService jwtService,
            IPasswordService passwordService,
            IEmailService emailService,
            ICodigoVerificacionService codigoService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _emailService = emailService;
            _codigoService = codigoService;
            _configuration = configuration;
            _logger = logger;
        }

        // ==================== PASO 1: INICIAR REGISTRO ====================
        [HttpPost("iniciar-registro")]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarRegistro([FromBody] IniciarRegistroDTO iniciarDTO)
        {
            try
            {
                _logger.LogInformation($"📨 Iniciando registro para: {iniciarDTO.Email}");

                // Validar modelo
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var email = iniciarDTO.Email.Trim().ToLower();

                // Verificar si el email ya está completamente registrado
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && u.EmailVerificado && u.Rol != "pendiente");

                if (usuarioExistente != null)
                    return BadRequest(new { mensaje = "El email ya está registrado y verificado" });

                // Generar código de verificación
                var codigo = _codigoService.GenerarCodigo6Digitos();
                var expiracion = DateTime.UtcNow.AddMinutes(15);

                // Buscar usuario temporal existente o crear uno nuevo
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && !u.EmailVerificado);

                if (usuario == null)
                {
                    // Crear usuario temporal
                    usuario = new Usuario
                    {
                        Email = email,
                        Rol = "pendiente",
                        Nombre = "Pendiente",
                        EmailVerificado = false,
                        CodigoVerificacion = codigo,
                        CodigoVerificacionExpiracion = expiracion,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Usuarios.Add(usuario);
                }
                else
                {
                    // Actualizar código existente
                    usuario.CodigoVerificacion = codigo;
                    usuario.CodigoVerificacionExpiracion = expiracion;
                    usuario.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Enviar código por email (asíncrono)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var enviado = await _emailService.EnviarCodigoVerificacionAsync(email, codigo);
                        if (enviado)
                        {
                            _logger.LogInformation($"✅ Código enviado a: {email}");
                        }
                        else
                        {
                            _logger.LogWarning($"⚠️ Error al enviar código a: {email}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Error enviando email a {email}");
                    }
                });

                return Ok(new
                {
                    mensaje = "Código de verificación enviado a tu email",
                    email = email,
                    expiraEnMinutos = 15,
                    recordatorio = "Revisa tu carpeta de spam si no ves el email"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en IniciarRegistro");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // ==================== PASO 2: VERIFICAR CÓDIGO ====================
        [HttpPost("verificar-codigo")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoDTO verificarDTO)
        {
            try
            {
                _logger.LogInformation($"🔍 Verificando código para: {verificarDTO.Email}");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var email = verificarDTO.Email.Trim().ToLower();
                var codigoIngresado = verificarDTO.Codigo.Trim();

                // Buscar usuario
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && !u.EmailVerificado);

                if (usuario == null)
                    return BadRequest(new { mensaje = "Registro no encontrado o ya verificado" });

                // Validar código (corregido: manejar nulo)
                if (!_codigoService.ValidarCodigo(
                    usuario.CodigoVerificacion ?? "", 
                    codigoIngresado, 
                    usuario.CodigoVerificacionExpiracion))
                {
                    return BadRequest(new { mensaje = "Código incorrecto o expirado" });
                }

                // Marcar como verificado
                usuario.EmailVerificado = true;
                usuario.FechaVerificacion = DateTime.UtcNow;
                usuario.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"✅ Email verificado: {email}");

                return Ok(new
                {
                    mensaje = "Email verificado exitosamente",
                    email = usuario.Email,
                    verificado = true,
                    siguientePaso = "completar-datos-personales"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en VerificarCodigo");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // ==================== PASO 3: COMPLETAR DATOS PERSONALES ====================
        [HttpPost("completar-datos-personales")]
        [AllowAnonymous]
        public async Task<IActionResult> CompletarDatosPersonales([FromBody] CompletarDatosPersonalesDTO datosDTO)
        {
            try
            {
                _logger.LogInformation($"👤 Completando datos personales para: {datosDTO.Email}");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var email = datosDTO.Email.Trim().ToLower();

                // Buscar usuario verificado
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && u.EmailVerificado && u.Rol == "pendiente");

                if (usuario == null)
                    return BadRequest(new { mensaje = "Email no verificado o no encontrado" });

                // Verificar código nuevamente (corregido: manejar nulo)
                if (!_codigoService.ValidarCodigo(
                    usuario.CodigoVerificacion ?? "",
                    datosDTO.CodigoVerificacion,
                    usuario.CodigoVerificacionExpiracion))
                {
                    return BadRequest(new { mensaje = "Código de verificación inválido" });
                }

                // Actualizar datos personales
                usuario.Nombre = datosDTO.Nombre.Trim();
                usuario.Apellidos = datosDTO.Apellidos.Trim();
                usuario.Telefono = datosDTO.Telefono.Trim();
                usuario.Rol = "pre-agricultor"; // Rol temporal hasta completar registro
                usuario.UpdatedAt = DateTime.UtcNow;

                // Limpiar código de verificación (ya se usó)
                usuario.CodigoVerificacion = null;
                usuario.CodigoVerificacionExpiracion = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"✅ Datos personales actualizados para: {email}");

                return Ok(new
                {
                    mensaje = "Datos personales guardados exitosamente",
                    email = usuario.Email,
                    nombre = usuario.Nombre,
                    siguientePaso = "completar-registro-agricultor"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en CompletarDatosPersonales");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // ==================== PASO 4: COMPLETAR REGISTRO AGRICULTOR ====================
        [HttpPost("completar-registro-agricultor")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> CompletarRegistroAgricultor([FromBody] CompletarRegistroAgricultorDTO registroDTO)
        {
            try
            {
                _logger.LogInformation($"🌱 Completando registro agricultor para: {registroDTO.Email}");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Validar términos y condiciones
                if (!registroDTO.AceptaTerminos)
                    return BadRequest(new { mensaje = "Debes aceptar los términos y condiciones" });

                var email = registroDTO.Email.Trim().ToLower();

                // Buscar usuario pre-registrado
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && u.Rol == "pre-agricultor" && u.EmailVerificado);

                if (usuario == null)
                    return BadRequest(new { mensaje = "Usuario no encontrado o datos personales no completados" });

                // Verificar que el DNI no exista
                if (await _context.Agricultores.AnyAsync(a => a.Dni == registroDTO.Dni))
                    return BadRequest(new { mensaje = "El DNI ya está registrado" });

                // Actualizar contraseña
                usuario.PasswordHash = _passwordService.HashPassword(registroDTO.Password);
                usuario.Rol = "agricultor";
                usuario.UpdatedAt = DateTime.UtcNow;

                // Crear agricultor
                var agricultor = new Agricultor
                {
                    UsuarioId = usuario.Id,
                    Dni = registroDTO.Dni.Trim(),
                    Direccion = registroDTO.Direccion.Trim(),
                    Experiencia = registroDTO.Experiencia?.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Agricultores.Add(agricultor);
                await _context.SaveChangesAsync();

                // Generar token JWT
                var token = _jwtService.GenerateToken(usuario);

                // Enviar email de bienvenida (asíncrono)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.EnviarBienvenidaAgricultorAsync(usuario.Email, usuario.Nombre);
                        _logger.LogInformation($"📧 Email de bienvenida enviado a: {usuario.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"⚠️ Error enviando bienvenida a {usuario.Email}");
                    }
                });

                // Preparar respuesta
                var response = new AuthResponseDTO
                {
                    Token = token,
                    Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")),
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Rol = usuario.Rol,
                        Nombre = usuario.Nombre,
                        Telefono = usuario.Telefono,
                        CreatedAt = usuario.CreatedAt,
                        UpdatedAt = usuario.UpdatedAt
                    }
                };

                _logger.LogInformation($"🎉 Registro completado exitosamente para: {usuario.Email}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en CompletarRegistroAgricultor");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // ==================== LOGIN EXISTENTE ====================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (usuario == null)
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });

                // Verificar que el email esté verificado y sea agricultor
                if (!usuario.EmailVerificado || usuario.Rol != "agricultor")
                    return Unauthorized(new { mensaje = "Usuario no verificado o no es agricultor" });

                if (!_passwordService.VerifyPassword(loginDTO.Password, usuario.PasswordHash))
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });

                var token = _jwtService.GenerateToken(usuario);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")),
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Rol = usuario.Rol,
                        Nombre = usuario.Nombre,
                        Telefono = usuario.Telefono,
                        CreatedAt = usuario.CreatedAt,
                        UpdatedAt = usuario.UpdatedAt
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en Login");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // ==================== REENVIAR CÓDIGO ====================
        [HttpPost("reenviar-codigo")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarCodigo([FromBody] IniciarRegistroDTO reenviarDTO)
        {
            // Reutilizar la lógica de iniciar registro
            return await IniciarRegistro(reenviarDTO);
        }
        
        [HttpGet("debug-email")]
        [AllowAnonymous]
        public IActionResult DebugEmailConfig()
        {
            try
            {
                var config = new
                {
                    // Configuración cargada
                    SmtpServer = _configuration["Email:SmtpServer"],
                    SmtpPort = _configuration["Email:SmtpPort"],
                    Usuario = _configuration["Email:Usuario"],
                    Password = string.IsNullOrEmpty(_configuration["Email:Password"]) 
                        ? "❌ NO CONFIGURADO" 
                        : "✅ CONFIGURADO (" + _configuration["Email:Password"]?.Length + " caracteres)",
                    DeEmail = _configuration["Email:DeEmail"],
                    UseSsl = _configuration["Email:UseSsl"],
            
                    // Archivo de configuración
                    ConfigFile = "appsettings.json",
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development"
                };
        
                return Ok(new
                {
                    mensaje = "Configuración de email detectada",
                    config = config,
                    status = string.IsNullOrEmpty(_configuration["Email:Password"]) 
                        ? "❌ ERROR: Password no configurado" 
                        : "✅ Configuración parece correcta"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al leer configuración", 
                    error = ex.Message,
                    stackTrace = ex.StackTrace 
                });
            }
        }
    }
}