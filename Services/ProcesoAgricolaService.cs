using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class ProcesoAgricolaService : IProcesoAgricolaService
    {
        private readonly AppDbContext _context;

        public ProcesoAgricolaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProcesoAgricola>> GetAllProcesosAsync()
        {
            return await _context.ProcesosAgricolas
                .Include(p => p.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(p => p.TipoProceso)
                .Include(p => p.Agricultor)
                .ToListAsync();
        }

        public async Task<ProcesoAgricola> GetProcesoByIdAsync(int id)
        {
            return await _context.ProcesosAgricolas
                .Include(p => p.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(p => p.TipoProceso)
                .Include(p => p.Agricultor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProcesoAgricola> CreateProcesoAsync(ProcesoAgricola proceso)
        {
            _context.ProcesosAgricolas.Add(proceso);
            await _context.SaveChangesAsync();
            return proceso;
        }

        public async Task<ProcesoAgricola> UpdateProcesoAsync(int id, ProcesoAgricola proceso)
        {
            var existingProceso = await _context.ProcesosAgricolas.FindAsync(id);
            if (existingProceso == null)
                return null;

            existingProceso.CultivoId = proceso.CultivoId;
            existingProceso.TipoProcesoId = proceso.TipoProcesoId;
            existingProceso.Fecha = proceso.Fecha;
            existingProceso.Descripcion = proceso.Descripcion;
            existingProceso.Costo = proceso.Costo;
            existingProceso.AgricultorId = proceso.AgricultorId;
            existingProceso.Observaciones = proceso.Observaciones;

            await _context.SaveChangesAsync();
            return existingProceso;
        }

        public async Task<bool> DeleteProcesoAsync(int id)
        {
            var proceso = await _context.ProcesosAgricolas.FindAsync(id);
            if (proceso == null)
                return false;

            _context.ProcesosAgricolas.Remove(proceso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProcesoAgricola>> GetProcesosByCultivoAsync(int cultivoId)
        {
            return await _context.ProcesosAgricolas
                .Include(p => p.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(p => p.TipoProceso)
                .Where(p => p.CultivoId == cultivoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProcesoAgricola>> GetProcesosByTipoAsync(int tipoProcesoId)
        {
            return await _context.ProcesosAgricolas
                .Include(p => p.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(p => p.TipoProceso)
                .Where(p => p.TipoProcesoId == tipoProcesoId)
                .ToListAsync();
        }
    }
}