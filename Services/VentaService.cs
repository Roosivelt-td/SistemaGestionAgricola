using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class VentaService : IVentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllVentasAsync()
        {
            return await _context.Ventas
                .Include(v => v.Comprador)
                .Include(v => v.Agricultor)
                .ToListAsync();
        }

        public async Task<Venta> GetVentaByIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Comprador)
                .Include(v => v.Agricultor)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venta> CreateVentaAsync(Venta venta)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return venta;
        }

        public async Task<Venta> UpdateVentaAsync(int id, Venta venta)
        {
            var existingVenta = await _context.Ventas.FindAsync(id);
            if (existingVenta == null)
                return null;

            existingVenta.CosechaId = venta.CosechaId;
            existingVenta.CompradorId = venta.CompradorId;
            existingVenta.AgricultorId = venta.AgricultorId;
            existingVenta.FechaVenta = venta.FechaVenta;
            existingVenta.Cantidad = venta.Cantidad;
            existingVenta.PrecioUnitario = venta.PrecioUnitario;
            existingVenta.Total = venta.Total;
            existingVenta.Estado = venta.Estado;

            await _context.SaveChangesAsync();
            return existingVenta;
        }

        public async Task<bool> DeleteVentaAsync(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return false;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Venta>> GetVentasByAgricultorAsync(int agricultorId)
        {
            return await _context.Ventas
                .Include(v => v.Comprador)
                .Include(v => v.Agricultor)
                .Where(v => v.AgricultorId == agricultorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venta>> GetVentasByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Ventas
                .Include(v => v.Comprador)
                .Include(v => v.Agricultor)
                .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
                .ToListAsync();
        }
    }
}