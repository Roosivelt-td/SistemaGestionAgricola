using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
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
                var usuario = await _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Rol = u.Rol,
                        Nombre = u.Nombre,
                        Telefono = u.Telefono,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado");
                }

                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(CreateUsuarioDTO createUsuarioDTO)
        {
            try
            {
                // Validar que los campos requeridos estén presentes
                if (string.IsNullOrWhiteSpace(createUsuarioDTO.Email) ||
                    string.IsNullOrWhiteSpace(createUsuarioDTO.Password) ||
                    string.IsNullOrWhiteSpace(createUsuarioDTO.Rol) ||
                    string.IsNullOrWhiteSpace(createUsuarioDTO.Nombre))
                {
                    return BadRequest("Email, Password, Rol y Nombre son campos requeridos");
                }

                // Validar rol
                if (!IsValidRol(createUsuarioDTO.Rol))
                {
                    return BadRequest("Rol no válido. Los roles permitidos son: admin, agricultor, supervisor");
                }

                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == createUsuarioDTO.Email))
                {
                    return BadRequest("El email ya está registrado");
                }

                // Crear nuevo usuario (las fechas se establecen en el constructor)
                var usuario = new Usuario
                {
                    Email = createUsuarioDTO.Email.Trim(),
                    Password = createUsuarioDTO.Password,
                    Rol = createUsuarioDTO.Rol.Trim(),
                    Nombre = createUsuarioDTO.Nombre.Trim(),
                    Telefono = createUsuarioDTO.Telefono?.Trim()
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Crear DTO de respuesta
                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    Nombre = usuario.Nombre,
                    Telefono = usuario.Telefono,
                    CreatedAt = usuario.CreatedAt,
                    UpdatedAt = usuario.UpdatedAt
                };

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioDTO);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UpdateUsuarioDTO updateUsuarioDTO)
        {
            try
            {
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
                    usuario.Password = updateUsuarioDTO.Password;

                if (updateUsuarioDTO.Rol != null)
                    usuario.Rol = updateUsuarioDTO.Rol.Trim();

                if (updateUsuarioDTO.Nombre != null)
                    usuario.Nombre = updateUsuarioDTO.Nombre.Trim();

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