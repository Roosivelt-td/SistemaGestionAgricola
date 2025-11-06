using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerrenosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TerrenosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Terrenos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TerrenoDTO>>> GetTerrenos()
        {
            try
            {
                var terrenos = await _context.Terrenos
                    .Include(t => t.Agricultor)
                        .ThenInclude(a => a.Usuario)
                    .Select(t => new TerrenoDTO
                    {
                        Id = t.Id,
                        AgricultorId = t.AgricultorId,
                        Nombre = t.Nombre,
                        Ubicacion = t.Ubicacion,
                        AreaHectareas = t.AreaHectareas,
                        TipoTenencia = t.TipoTenencia,
                        CostoAlquiler = t.CostoAlquiler,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt,
                        AgricultorNombre = t.Agricultor.Usuario.Nombre,
                        AgricultorDni = t.Agricultor.Dni,
                        UsuarioNombre = t.Agricultor.Usuario.Nombre
                    })
                    .ToListAsync();

                return Ok(terrenos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Terrenos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TerrenoDTO>> GetTerreno(int id)
        {
            try
            {
                var terreno = await _context.Terrenos
                    .Include(t => t.Agricultor)
                        .ThenInclude(a => a.Usuario)
                    .Where(t => t.Id == id)
                    .Select(t => new TerrenoDTO
                    {
                        Id = t.Id,
                        AgricultorId = t.AgricultorId,
                        Nombre = t.Nombre,
                        Ubicacion = t.Ubicacion,
                        AreaHectareas = t.AreaHectareas,
                        TipoTenencia = t.TipoTenencia,
                        CostoAlquiler = t.CostoAlquiler,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt,
                        AgricultorNombre = t.Agricultor.Usuario.Nombre,
                        AgricultorDni = t.Agricultor.Dni,
                        UsuarioNombre = t.Agricultor.Usuario.Nombre
                    })
                    .FirstOrDefaultAsync();

                if (terreno == null)
                {
                    return NotFound($"Terreno con ID {id} no encontrado");
                }

                return terreno;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Terrenos/agricultor/5
        [HttpGet("agricultor/{agricultorId}")]
        public async Task<ActionResult<IEnumerable<TerrenoDTO>>> GetTerrenosByAgricultor(int agricultorId)
        {
            try
            {
                var terrenos = await _context.Terrenos
                    .Include(t => t.Agricultor)
                        .ThenInclude(a => a.Usuario)
                    .Where(t => t.AgricultorId == agricultorId)
                    .Select(t => new TerrenoDTO
                    {
                        Id = t.Id,
                        AgricultorId = t.AgricultorId,
                        Nombre = t.Nombre,
                        Ubicacion = t.Ubicacion,
                        AreaHectareas = t.AreaHectareas,
                        TipoTenencia = t.TipoTenencia,
                        CostoAlquiler = t.CostoAlquiler,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt,
                        AgricultorNombre = t.Agricultor.Usuario.Nombre,
                        AgricultorDni = t.Agricultor.Dni,
                        UsuarioNombre = t.Agricultor.Usuario.Nombre
                    })
                    .ToListAsync();

                return Ok(terrenos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/Terrenos
        [HttpPost]
        public async Task<ActionResult<TerrenoDTO>> PostTerreno(CreateTerrenoDTO createTerrenoDTO)
        {
            try
            {
                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(createTerrenoDTO.Nombre))
                {
                    return BadRequest("Nombre es un campo requerido");
                }

                if (string.IsNullOrWhiteSpace(createTerrenoDTO.TipoTenencia))
                {
                    return BadRequest("TipoTenencia es un campo requerido");
                }

                // Validar tipo de tenencia
                if (!IsValidTipoTenencia(createTerrenoDTO.TipoTenencia))
                {
                    return BadRequest("Tipo de tenencia no válido. Los valores permitidos son: propio, alquilado");
                }

                // Verificar si el agricultor existe
                var agricultor = await _context.Agricultores
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == createTerrenoDTO.AgricultorId);
                
                if (agricultor == null)
                {
                    return BadRequest("El agricultor especificado no existe");
                }

                // Validar costo de alquiler según tipo de tenencia
                if (createTerrenoDTO.TipoTenencia == "propio" && createTerrenoDTO.CostoAlquiler > 0)
                {
                    return BadRequest("Un terreno propio no puede tener costo de alquiler");
                }

                if (createTerrenoDTO.TipoTenencia == "alquilado" && createTerrenoDTO.CostoAlquiler <= 0)
                {
                    return BadRequest("Un terreno alquilado debe tener un costo de alquiler mayor a 0");
                }

                var terreno = new Terreno
                {
                    AgricultorId = createTerrenoDTO.AgricultorId,
                    Nombre = createTerrenoDTO.Nombre.Trim(),
                    Ubicacion = createTerrenoDTO.Ubicacion?.Trim(),
                    AreaHectareas = createTerrenoDTO.AreaHectareas,
                    TipoTenencia = createTerrenoDTO.TipoTenencia.Trim(),
                    CostoAlquiler = createTerrenoDTO.CostoAlquiler
                };

                _context.Terrenos.Add(terreno);
                await _context.SaveChangesAsync();

                var terrenoDTO = new TerrenoDTO
                {
                    Id = terreno.Id,
                    AgricultorId = terreno.AgricultorId,
                    Nombre = terreno.Nombre,
                    Ubicacion = terreno.Ubicacion,
                    AreaHectareas = terreno.AreaHectareas,
                    TipoTenencia = terreno.TipoTenencia,
                    CostoAlquiler = terreno.CostoAlquiler,
                    CreatedAt = terreno.CreatedAt,
                    UpdatedAt = terreno.UpdatedAt,
                    AgricultorNombre = agricultor.Usuario.Nombre,
                    AgricultorDni = agricultor.Dni,
                    UsuarioNombre = agricultor.Usuario.Nombre
                };

                return CreatedAtAction(nameof(GetTerreno), new { id = terreno.Id }, terrenoDTO);
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

        // PUT: api/Terrenos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerreno(int id, UpdateTerrenoDTO updateTerrenoDTO)
        {
            try
            {
                var terreno = await _context.Terrenos.FindAsync(id);
                if (terreno == null)
                {
                    return NotFound($"Terreno con ID {id} no encontrado");
                }

                // Validar tipo de tenencia si se está actualizando
                if (updateTerrenoDTO.TipoTenencia != null && !IsValidTipoTenencia(updateTerrenoDTO.TipoTenencia))
                {
                    return BadRequest("Tipo de tenencia no válido. Los valores permitidos son: propio, alquilado");
                }

                // Actualizar solo los campos que se proporcionaron
                if (updateTerrenoDTO.Nombre != null)
                    terreno.Nombre = updateTerrenoDTO.Nombre.Trim();

                if (updateTerrenoDTO.Ubicacion != null)
                    terreno.Ubicacion = updateTerrenoDTO.Ubicacion.Trim();

                if (updateTerrenoDTO.AreaHectareas.HasValue)
                    terreno.AreaHectareas = updateTerrenoDTO.AreaHectareas.Value;

                if (updateTerrenoDTO.TipoTenencia != null)
                    terreno.TipoTenencia = updateTerrenoDTO.TipoTenencia.Trim();

                if (updateTerrenoDTO.CostoAlquiler.HasValue)
                    terreno.CostoAlquiler = updateTerrenoDTO.CostoAlquiler.Value;

                // Validar consistencia entre tipo de tenencia y costo de alquiler
                if (terreno.TipoTenencia == "propio" && terreno.CostoAlquiler > 0)
                {
                    return BadRequest("Un terreno propio no puede tener costo de alquiler");
                }

                if (terreno.TipoTenencia == "alquilado" && terreno.CostoAlquiler <= 0)
                {
                    return BadRequest("Un terreno alquilado debe tener un costo de alquiler mayor a 0");
                }

                terreno.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerrenoExists(id))
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

        // DELETE: api/Terrenos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerreno(int id)
        {
            try
            {
                var terreno = await _context.Terrenos.FindAsync(id);
                if (terreno == null)
                {
                    return NotFound($"Terreno con ID {id} no encontrado");
                }

                _context.Terrenos.Remove(terreno);
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

        private bool TerrenoExists(int id)
        {
            return _context.Terrenos.Any(e => e.Id == id);
        }

        private bool IsValidTipoTenencia(string tipoTenencia)
        {
            return tipoTenencia == "propio" || tipoTenencia == "alquilado";
        }
    }
}