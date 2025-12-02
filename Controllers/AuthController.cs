// 📁 Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;
using SistemaGestionAgricola.Services;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IConfiguration _configuration;

        public AuthController(
            AppDbContext context,
            IJwtService jwtService, 
            IPasswordService passwordService,
            IPasswordValidator passwordValidator, // ← Correcto
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _passwordValidator = passwordValidator; // ← CORRECCIÓN: Solo asignación
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(loginDTO.Email))
                    return BadRequest(new { 
                        success = false, 
                        message = "El email es requerido" 
                    });

                if (string.IsNullOrWhiteSpace(loginDTO.Password))
                    return BadRequest(new { 
                        success = false, 
                        message = "La contraseña es requerida" 
                    });

                // Validar formato de email
                if (!IsValidEmail(loginDTO.Email))
                    return BadRequest(new { 
                        success = false, 
                        message = "El formato del email no es válido" 
                    });

                // Buscar usuario por email
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (usuario == null)
                    return Unauthorized(new { 
                        success = false, 
                        message = "Credenciales inválidas. Verifica tu email y contraseña." 
                    });

                // Verificar contraseña usando PasswordService
                if (!_passwordService.VerifyPassword(loginDTO.Password, usuario.PasswordHash))
                    return Unauthorized(new { 
                        success = false, 
                        message = "Credenciales inválidas. Verifica tu email y contraseña." 
                    });

                // Generar token
                var token = _jwtService.GenerateToken(usuario);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")), // Usar UtcNow
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Rol = usuario.Rol,
                        Nombre = usuario.Nombre,
                        Apellidos = usuario.Apellidos ?? string.Empty, // Manejar null
                        Telefono = usuario.Telefono,
                        CreatedAt = usuario.CreatedAt,
                        UpdatedAt = usuario.UpdatedAt
                    }
                };

                return Ok(new {
                    success = true,
                    message = "✅ Inicio de sesión exitoso",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = $"Error interno del servidor: {ex.Message}" 
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
        {
            try
            {
                // Validar que el email no exista
                if (await _context.Usuarios.AnyAsync(u => u.Email == registerDTO.Email))
                    return BadRequest(new { 
                        success = false, 
                        message = "El email ya está registrado" 
                    });
                
                // Validar formato de email
                if (!IsValidEmail(registerDTO.Email))
                    return BadRequest(new { 
                        success = false, 
                        message = "El formato del email no es válido" 
                    });

                // Validar rol
                if (!IsValidRol(registerDTO.Rol))
                    return BadRequest(new { 
                        success = false, 
                        message = $"Rol '{registerDTO.Rol}' no válido. Roles permitidos: admin, agricultor, supervisor" 
                    });

                // VALIDACIÓN DE CONTRASEÑA SEGURA
                var passwordValidation = _passwordValidator.ValidatePassword(registerDTO.Password);
                if (!passwordValidation.IsValid)
                    return BadRequest(new { 
                        success = false, 
                        message = passwordValidation.Message 
                    });
                
                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(registerDTO.Nombre))
                    return BadRequest(new { 
                        success = false, 
                        message = "El nombre es requerido" 
                    });

                // Crear usuario CON HASH
                var usuario = new Usuario
                {
                    Email = registerDTO.Email.Trim(),
                    PasswordHash = _passwordService.HashPassword(registerDTO.Password),
                    Rol = registerDTO.Rol.Trim(),
                    Nombre = registerDTO.Nombre.Trim(),
                    Apellidos = registerDTO.Apellidos?.Trim() ?? string.Empty,
                    Telefono = registerDTO.Telefono?.Trim()
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Generar token
                var token = _jwtService.GenerateToken(usuario);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")), // Usar UtcNow
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Rol = usuario.Rol,
                        Nombre = usuario.Nombre,
                        Apellidos = usuario.Apellidos ?? string.Empty, // Manejar null
                        Telefono = usuario.Telefono,
                        CreatedAt = usuario.CreatedAt,
                        UpdatedAt = usuario.UpdatedAt
                    }
                };

                return Ok(new {
                    success = true,
                    message = "✅ Usuario registrado exitosamente",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = $"Error interno del servidor: {ex.Message}" 
                });
            }
        }

        private bool IsValidRol(string rol)
        {
            return rol == "admin" || rol == "agricultor" || rol == "supervisor";
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}