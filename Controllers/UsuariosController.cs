using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ← Proteger todo el controller
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        [Authorize(Roles = "admin")] // ← Solo admin puede ver todos los usuarios
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Rol = u.Rol,
                        Nombre = u.Nombre,
                        Apellidos = u.Apellidos,
                        Telefono = u.Telefono,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            try
            {
                // Obtener el ID del usuario actual de múltiples formas
                var currentUserId = 0;
        
                if (User.FindFirst("userId") != null)
                    currentUserId = int.Parse(User.FindFirst("userId").Value);
                else if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
                    currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserId != id && currentUserRole != "admin")
                    return Forbid();
        
                var usuario = await _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Rol = u.Rol,
                        Nombre = u.Nombre,
                        Apellidos = u.Apellidos,
                        Telefono = u.Telefono,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                    return NotFound($"Usuario con ID {id} no encontrado");

                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        // GET: api/Usuarios/profile
        [HttpGet("profile")]
        public async Task<ActionResult<UsuarioDTO>> GetProfile()
        {
            try
            {
                Console.WriteLine("🔍 INICIANDO GETPROFILE ================");
                
                // Obtener todos los claims disponibles para diagnóstico
                var claims = User.Claims.ToList();
                Console.WriteLine($"🔍 Total de claims: {claims.Count}");
                foreach (var claim in claims)
                {
                    Console.WriteLine($"🔍 Claim: {claim.Type} = {claim.Value}");
                }
                
                // Intentar obtener el ID de múltiples formas
                var userId = 0;
                
                // Opción 1: Buscar por "userId" (custom claim)
                var userIdClaim = User.FindFirst("userId")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim))
                {
                    Console.WriteLine($"🔍 Encontrado userId claim: {userIdClaim}");
                    userId = int.Parse(userIdClaim);
                }
                // Opción 2: Buscar por NameIdentifier (standard claim)
                else if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    Console.WriteLine($"🔍 Encontrado NameIdentifier claim: {nameIdentifier}");
                    userId = int.Parse(nameIdentifier);
                }
                // Opción 3: Buscar por email y obtener ID
                else if (User.FindFirst(ClaimTypes.Email) != null)
                {
                    var email = User.FindFirst(ClaimTypes.Email).Value;
                    Console.WriteLine($"🔍 Buscando por email: {email}");
                    var usuarioByEmail = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == email);
                        
                    if (usuarioByEmail != null)
                    {
                        userId = usuarioByEmail.Id;
                        Console.WriteLine($"🔍 Usuario encontrado por email, ID: {userId}");
                    }
                }

                if (userId == 0)
                {
                    Console.WriteLine("❌ No se pudo obtener el ID del usuario de ningún claim");
                    return Unauthorized("No se pudo identificar al usuario");
                }

                Console.WriteLine($"🔍 Buscando usuario con ID: {userId}");

                var usuario = await _context.Usuarios
                    .Where(u => u.Id == userId)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Rol = u.Rol,
                        Nombre = u.Nombre,
                        Apellidos = u.Apellidos,
                        Telefono = u.Telefono,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    Console.WriteLine($"❌ Usuario con ID {userId} no encontrado en BD");
                    
                    // Verificar qué usuarios existen para diagnóstico
                    var todosUsuarios = await _context.Usuarios
                        .Select(u => new { u.Id, u.Email, u.Rol })
                        .ToListAsync();
                        
                    Console.WriteLine($"🔍 Usuarios en BD: {todosUsuarios.Count}");
                    foreach (var u in todosUsuarios)
                    {
                        Console.WriteLine($"🔍 - ID: {u.Id}, Email: {u.Email}, Rol: {u.Rol}");
                    }
                    
                    return NotFound("Usuario no encontrado");
                }

                Console.WriteLine($"✅ Usuario encontrado: {usuario.Email} (ID: {usuario.Id})");
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 ERROR en GetProfile: {ex.Message}");
                Console.WriteLine($"💥 StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UpdateUsuarioDTO updateUsuarioDTO)
        {
            try
            {
                // Solo permitir actualizar el propio perfil o si es admin
                var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserId != id && currentUserRole != "admin")
                    return Forbid();

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado");
                }

                // Validar rol si se está actualizando
                if (updateUsuarioDTO.Rol != null && !IsValidRol(updateUsuarioDTO.Rol))
                {
                    return BadRequest("Rol no válido. Los roles permitidos son: admin, agricultor, supervisor");
                }

                // Verificar si el email ya existe (si se está actualizando)
                if (updateUsuarioDTO.Email != null && updateUsuarioDTO.Email != usuario.Email)
                {
                    if (await _context.Usuarios.AnyAsync(u => u.Email == updateUsuarioDTO.Email && u.Id != id))
                    {
                        return BadRequest("El email ya está registrado por otro usuario");
                    }
                    usuario.Email = updateUsuarioDTO.Email.Trim();
                }

                // Actualizar solo los campos que se proporcionaron
                if (updateUsuarioDTO.Password != null)
                    usuario.PasswordHash = updateUsuarioDTO.Password;

                if (updateUsuarioDTO.Rol != null)
                    usuario.Rol = updateUsuarioDTO.Rol.Trim();

                if (updateUsuarioDTO.Nombre != null)
                    usuario.Nombre = updateUsuarioDTO.Nombre.Trim();

                if (updateUsuarioDTO.Apellidos != null)
                    usuario.Apellidos = updateUsuarioDTO.Apellidos.Trim();

                if (updateUsuarioDTO.Telefono != null)
                    usuario.Telefono = updateUsuarioDTO.Telefono.Trim();

                // Actualizar fecha de modificación
                usuario.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error al actualizar en la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] // ← Solo admin puede eliminar usuarios
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado");
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error al eliminar en la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private bool IsValidRol(string rol)
        {
            return rol == "admin" || rol == "agricultor" || rol == "supervisor";
        }
    }
}