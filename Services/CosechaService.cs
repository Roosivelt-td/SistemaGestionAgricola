using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class CosechaService : ICosechaService
    {
        private readonly AppDbContext _context;

        public CosechaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cosecha>> GetAllCosechasAsync()
        {
            return await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(c => c.Agricultor)
                .ToListAsync();
        }

        public async Task<Cosecha> GetCosechaByIdAsync(int id)
        {
            return await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(c => c.Agricultor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cosecha> CreateCosechaAsync(Cosecha cosecha)
        {
            _context.Cosechas.Add(cosecha);
            await _context.SaveChangesAsync();
            return cosecha;
        }

        public async Task<Cosecha> UpdateCosechaAsync(int id, Cosecha cosecha)
        {
            var existingCosecha = await _context.Cosechas.FindAsync(id);
            if (existingCosecha == null)
                return null;

            existingCosecha.CultivoId = cosecha.CultivoId;
            existingCosecha.AgricultorId = cosecha.AgricultorId;
            existingCosecha.FechaCosecha = cosecha.FechaCosecha;
            existingCosecha.Cantidad = cosecha.Cantidad;
            existingCosecha.UnidadMedida = cosecha.UnidadMedida;
            existingCosecha.Calidad = cosecha.Calidad;
            existingCosecha.Observaciones = cosecha.Observaciones;

            await _context.SaveChangesAsync();
            return existingCosecha;
        }

        public async Task<bool> DeleteCosechaAsync(int id)
        {
            var cosecha = await _context.Cosechas.FindAsync(id);
            if (cosecha == null)
                return false;

            _context.Cosechas.Remove(cosecha);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Cosecha>> GetCosechasByCultivoAsync(int cultivoId)
        {
            return await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Where(c => c.CultivoId == cultivoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cosecha>> GetCosechasByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(c => c.Agricultor)
                .Where(c => c.FechaCosecha >= fechaInicio && c.FechaCosecha <= fechaFin)
                .ToListAsync();
        }
    }
}