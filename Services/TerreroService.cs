using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class TerreroService : ITerreroService
    {
        private readonly AppDbContext _context;

        public TerreroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Terreno>> GetAllTerrenosAsync()
        {
            return await _context.Terrenos
                .Include(t => t.Agricultor)
                .ToListAsync();
        }

        public async Task<Terreno> GetTerrenoByIdAsync(int id)
        {
            return await _context.Terrenos
                .Include(t => t.Agricultor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Terreno> CreateTerrenoAsync(Terreno terreno)
        {
            _context.Terrenos.Add(terreno);
            await _context.SaveChangesAsync();
            return terreno;
        }

        public async Task<Terreno> UpdateTerrenoAsync(int id, Terreno terreno)
        {
            var existingTerreno = await _context.Terrenos.FindAsync(id);
            if (existingTerreno == null)
                return null;

            existingTerreno.Nombre = terreno.Nombre;
            existingTerreno.Ubicacion = terreno.Ubicacion;
            existingTerreno.Area = terreno.Area;
            existingTerreno.Coordenadas = terreno.Coordenadas;
            existingTerreno.Descripcion = terreno.Descripcion;
            existingTerreno.AgricultorId = terreno.AgricultorId;
            existingTerreno.Activo = terreno.Activo;

            await _context.SaveChangesAsync();
            return existingTerreno;
        }

        public async Task<bool> DeleteTerrenoAsync(int id)
        {
            var terreno = await _context.Terrenos.FindAsync(id);
            if (terreno == null)
                return false;

            _context.Terrenos.Remove(terreno);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Terreno>> GetTerrenosByAgricultorAsync(int agricultorId)
        {
            return await _context.Terrenos
                .Where(t => t.AgricultorId == agricultorId)
                .ToListAsync();
        }
    }
}