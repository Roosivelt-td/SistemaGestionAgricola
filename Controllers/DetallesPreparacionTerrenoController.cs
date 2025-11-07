using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPreparacionTerrenoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetallesPreparacionTerrenoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DetallesPreparacionTerreno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePreparacionTerrenoDTO>>> GetDetallesPreparacionTerreno()
        {
            try
            {
                var detalles = await _context.DetallesPreparacionTerreno
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.TipoProceso)
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.Cultivo)
                            .ThenInclude(c => c.Terreno)
                                .ThenInclude(t => t.Agricultor)
                                    .ThenInclude(a => a.Usuario)
                    .Select(d => new DetallePreparacionTerrenoDTO
                    {
                        Id = d.Id,
                        ProcesoId = d.ProcesoId,
                        TipoPreparacion = d.TipoPreparacion,
                        HorasMaquinaria = d.HorasMaquinaria,
                        Costo = d.Costo,
                        Observaciones = d.Observaciones,
                        CreatedAt = d.CreatedAt,
                        ProcesoTipo = d.ProcesoAgricola.TipoProceso.Nombre,
                        CultivoNombre = d.ProcesoAgricola.Cultivo.TipoCultivo.Nombre,
                        TerrenoNombre = d.ProcesoAgricola.Cultivo.Terreno.Nombre,
                        AgricultorNombre = d.ProcesoAgricola.Cultivo.Terreno.Agricultor.Usuario.Nombre
                    })
                    .ToListAsync();

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/DetallesPreparacionTerreno/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePreparacionTerrenoDTO>> GetDetallePreparacionTerreno(int id)
        {
            try
            {
                var detalle = await _context.DetallesPreparacionTerreno
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.TipoProceso)
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.Cultivo)
                            .ThenInclude(c => c.Terreno)
                                .ThenInclude(t => t.Agricultor)
                                    .ThenInclude(a => a.Usuario)
                    .Where(d => d.Id == id)
                    .Select(d => new DetallePreparacionTerrenoDTO
                    {
                        Id = d.Id,
                        ProcesoId = d.ProcesoId,
                        TipoPreparacion = d.TipoPreparacion,
                        HorasMaquinaria = d.HorasMaquinaria,
                        Costo = d.Costo,
                        Observaciones = d.Observaciones,
                        CreatedAt = d.CreatedAt,
                        ProcesoTipo = d.ProcesoAgricola.TipoProceso.Nombre,
                        CultivoNombre = d.ProcesoAgricola.Cultivo.TipoCultivo.Nombre,
                        TerrenoNombre = d.ProcesoAgricola.Cultivo.Terreno.Nombre,
                        AgricultorNombre = d.ProcesoAgricola.Cultivo.Terreno.Agricultor.Usuario.Nombre
                    })
                    .FirstOrDefaultAsync();

                if (detalle == null)
                {
                    return NotFound($"Detalle de preparación de terreno con ID {id} no encontrado");
                }

                return detalle;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/DetallesPreparacionTerreno/proceso/5
        [HttpGet("proceso/{procesoId}")]
        public async Task<ActionResult<IEnumerable<DetallePreparacionTerrenoDTO>>> GetDetallesByProceso(int procesoId)
        {
            try
            {
                var detalles = await _context.DetallesPreparacionTerreno
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.TipoProceso)
                    .Include(d => d.ProcesoAgricola)
                        .ThenInclude(pa => pa.Cultivo)
                            .ThenInclude(c => c.Terreno)
                                .ThenInclude(t => t.Agricultor)
                                    .ThenInclude(a => a.Usuario)
                    .Where(d => d.ProcesoId == procesoId)
                    .Select(d => new DetallePreparacionTerrenoDTO
                    {
                        Id = d.Id,
                        ProcesoId = d.ProcesoId,
                        TipoPreparacion = d.TipoPreparacion,
                        HorasMaquinaria = d.HorasMaquinaria,
                        Costo = d.Costo,
                        Observaciones = d.Observaciones,
                        CreatedAt = d.CreatedAt,
                        ProcesoTipo = d.ProcesoAgricola.TipoProceso.Nombre,
                        CultivoNombre = d.ProcesoAgricola.Cultivo.TipoCultivo.Nombre,
                        TerrenoNombre = d.ProcesoAgricola.Cultivo.Terreno.Nombre,
                        AgricultorNombre = d.ProcesoAgricola.Cultivo.Terreno.Agricultor.Usuario.Nombre
                    })
                    .ToListAsync();

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/DetallesPreparacionTerreno
[HttpPost]
public async Task<ActionResult<DetallePreparacionTerrenoDTO>> PostDetallePreparacionTerreno(CreateDetallePreparacionTerrenoDTO createDetalleDTO)
{
    try
    {
        // Validar campos requeridos
        if (string.IsNullOrWhiteSpace(createDetalleDTO.TipoPreparacion))
        {
            return BadRequest("TipoPreparacion es un campo requerido");
        }

        // Verificar si el proceso existe
        var proceso = await _context.ProcesosAgricolas
            .Include(pa => pa.TipoProceso)
            .Include(pa => pa.Cultivo)
                .ThenInclude(c => c.Terreno)
                    .ThenInclude(t => t.Agricultor)
                        .ThenInclude(a => a.Usuario)
            .FirstOrDefaultAsync(pa => pa.Id == createDetalleDTO.ProcesoId);
        
        if (proceso == null)
        {
            return BadRequest("El proceso especificado no existe");
        }

        // Validación FLEXIBLE - acepta diferentes nombres para preparación de terreno
        var tiposPreparacionValidos = new[] 
        { 
            "preparación terreno", 
            "preparacion terreno", 
            "preparación del terreno", 
            "preparacion del terreno",
            "preparar terreno",
            "preparación de terreno",
            "preparacion de terreno"
        };

        var nombreTipoProceso = proceso.TipoProceso.Nombre.ToLower().Trim();
        
        if (!tiposPreparacionValidos.Contains(nombreTipoProceso))
        {
            return BadRequest($"Solo se pueden agregar detalles de preparación a procesos de tipo preparación de terreno. El proceso actual es: '{proceso.TipoProceso.Nombre}' con ID: {proceso.TipoProcesoId}");
        }

        var detalle = new DetallePreparacionTerreno
        {
            ProcesoId = createDetalleDTO.ProcesoId,
            TipoPreparacion = createDetalleDTO.TipoPreparacion.Trim(),
            HorasMaquinaria = createDetalleDTO.HorasMaquinaria,
            Costo = createDetalleDTO.Costo,
            Observaciones = createDetalleDTO.Observaciones?.Trim()
        };

        _context.DetallesPreparacionTerreno.Add(detalle);
        await _context.SaveChangesAsync();

        var detalleDTO = new DetallePreparacionTerrenoDTO
        {
            Id = detalle.Id,
            ProcesoId = detalle.ProcesoId,
            TipoPreparacion = detalle.TipoPreparacion,
            HorasMaquinaria = detalle.HorasMaquinaria,
            Costo = detalle.Costo,
            Observaciones = detalle.Observaciones,
            CreatedAt = detalle.CreatedAt,
            ProcesoTipo = proceso.TipoProceso.Nombre,
            CultivoNombre = proceso.Cultivo.TipoCultivo.Nombre,
            TerrenoNombre = proceso.Cultivo.Terreno.Nombre,
            AgricultorNombre = proceso.Cultivo.Terreno.Agricultor.Usuario.Nombre
        };

        return CreatedAtAction(nameof(GetDetallePreparacionTerreno), new { id = detalle.Id }, detalleDTO);
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

        // PUT: api/DetallesPreparacionTerreno/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePreparacionTerreno(int id, UpdateDetallePreparacionTerrenoDTO updateDetalleDTO)
        {
            try
            {
                var detalle = await _context.DetallesPreparacionTerreno.FindAsync(id);
                if (detalle == null)
                {
                    return NotFound($"Detalle de preparación de terreno con ID {id} no encontrado");
                }

                // Actualizar solo los campos que se proporcionaron
                if (updateDetalleDTO.TipoPreparacion != null)
                    detalle.TipoPreparacion = updateDetalleDTO.TipoPreparacion.Trim();

                if (updateDetalleDTO.HorasMaquinaria.HasValue)
                    detalle.HorasMaquinaria = updateDetalleDTO.HorasMaquinaria.Value;

                if (updateDetalleDTO.Costo.HasValue)
                    detalle.Costo = updateDetalleDTO.Costo.Value;

                if (updateDetalleDTO.Observaciones != null)
                    detalle.Observaciones = updateDetalleDTO.Observaciones.Trim();

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePreparacionTerrenoExists(id))
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

        // DELETE: api/DetallesPreparacionTerreno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePreparacionTerreno(int id)
        {
            try
            {
                var detalle = await _context.DetallesPreparacionTerreno.FindAsync(id);
                if (detalle == null)
                {
                    return NotFound($"Detalle de preparación de terreno con ID {id} no encontrado");
                }

                _context.DetallesPreparacionTerreno.Remove(detalle);
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

        private bool DetallePreparacionTerrenoExists(int id)
        {
            return _context.DetallesPreparacionTerreno.Any(e => e.Id == id);
        }
    }
}