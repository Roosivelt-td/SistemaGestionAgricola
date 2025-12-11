using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class CultivoService : ICultivoService
    {
        private readonly AppDbContext _context;

        public CultivoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cultivo>> GetAllCultivosAsync()
        {
            return await _context.Cultivos
                .Include(c => c.TipoCultivo)
                .Include(c => c.Terreno)
                .Include(c => c.Agricultor)
                .ToListAsync();
        }

        public async Task<Cultivo> GetCultivoByIdAsync(int id)
        {
            return await _context.Cultivos
                .Include(c => c.TipoCultivo)
                .Include(c => c.Terreno)
                .Include(c => c.Agricultor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cultivo> CreateCultivoAsync(Cultivo cultivo)
        {
            _context.Cultivos.Add(cultivo);
            await _context.SaveChangesAsync();
            return cultivo;
        }

        public async Task<Cultivo> UpdateCultivoAsync(int id, Cultivo cultivo)
        {
            var existingCultivo = await _context.Cultivos.FindAsync(id);
            if (existingCultivo == null)
                return null;

            existingCultivo.Nombre = cultivo.Nombre;
            existingCultivo.TipoCultivoId = cultivo.TipoCultivoId;
            existingCultivo.TerrenoId = cultivo.TerrenoId;
            existingCultivo.AgricultorId = cultivo.AgricultorId;
            existingCultivo.FechaSiembra = cultivo.FechaSiembra;
            existingCultivo.FechaCosechaEsperada = cultivo.FechaCosechaEsperada;
            existingCultivo.Estado = cultivo.Estado;
            existingCultivo.Area = cultivo.Area;
            existingCultivo.Activo = cultivo.Activo;

            await _context.SaveChangesAsync();
            return existingCultivo;
        }

        public async Task<bool> DeleteCultivoAsync(int id)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);
            if (cultivo == null)
                return false;

            _context.Cultivos.Remove(cultivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Cultivo>> GetCultivosByAgricultorAsync(int agricultorId)
        {
            return await _context.Cultivos
                .Include(c => c.TipoCultivo)
                .Include(c => c.Terreno)
                .Where(c => c.AgricultorId == agricultorId)
                .ToListAsync();
        }
    }
}