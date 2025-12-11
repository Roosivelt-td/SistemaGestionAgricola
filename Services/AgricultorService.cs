using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class AgricultorService : IAgricultorService
    {
        private readonly AppDbContext _context;

        public AgricultorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agricultor>> GetAllAgricultoresAsync()
        {
            return await _context.Agricultores.ToListAsync();
        }

        public async Task<Agricultor> GetAgricultorByIdAsync(int id)
        {
            return await _context.Agricultores
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agricultor> GetAgricultorByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Agricultores
                .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
        }

        public async Task<Agricultor> CreateAgricultorAsync(Agricultor agricultor)
        {
            _context.Agricultores.Add(agricultor);
            await _context.SaveChangesAsync();
            return agricultor;
        }

        public async Task<Agricultor> UpdateAgricultorAsync(int id, Agricultor agricultor)
        {
            var existingAgricultor = await _context.Agricultores.FindAsync(id);
            if (existingAgricultor == null)
                return null;

            existingAgricultor.Nombre = agricultor.Nombre;
            existingAgricultor.Apellidos = agricultor.Apellidos;
            existingAgricultor.Direccion = agricultor.Direccion;
            existingAgricultor.Telefono = agricultor.Telefono;
            existingAgricultor.FechaRegistro = agricultor.FechaRegistro;
            existingAgricultor.Activo = agricultor.Activo;

            await _context.SaveChangesAsync();
            return existingAgricultor;
        }

        public async Task<bool> DeleteAgricultorAsync(int id)
        {
            var agricultor = await _context.Agricultores.FindAsync(id);
            if (agricultor == null)
                return false;

            _context.Agricultores.Remove(agricultor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}