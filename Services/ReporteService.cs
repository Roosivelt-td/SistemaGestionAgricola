using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Interfaces;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Services
{
    public class ReporteService : IReporteService
    {
        private readonly AppDbContext _context;

        public ReporteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var stats = new DashboardStatsDto
            {
                TotalCultivos = await _context.Cultivos.CountAsync(),
                TotalCosechas = await _context.Cosechas.CountAsync(),
                TotalVentas = await _context.Ventas.CountAsync(),
                TotalAgricultores = await _context.Agricultores.CountAsync()
            };

            return stats;
        }

        public async Task<IEnumerable<ProduccionAnualDto>> GetProduccionAnualAsync(int year)
        {
            var produccion = await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Where(c => c.FechaCosecha.Year == year)
                .GroupBy(c => c.Cultivo.TipoCultivo.Nombre)
                .Select(g => new ProduccionAnualDto
                {
                    TipoCultivo = g.Key,
                    CantidadTotal = g.Sum(c => c.Cantidad),
                    UnidadMedida = g.First().UnidadMedida
                })
                .ToListAsync();

            return produccion;
        }

        public async Task<IEnumerable<RendimientoCultivoDto>> GetRendimientoPorCultivoAsync(int agricultorId)
        {
            var rendimiento = await _context.Cosechas
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.TipoCultivo)
                .Include(c => c.Cultivo)
                .ThenInclude(c => c.Terreno)
                .Where(c => c.AgricultorId == agricultorId)
                .GroupBy(c => new { c.Cultivo.TipoCultivo.Nombre, c.Cultivo.Nombre })
                .Select(g => new RendimientoCultivoDto
                {
                    TipoCultivo = g.Key.Nombre,
                    NombreCultivo = g.Key.Nombre,
                    CantidadTotal = g.Sum(c => c.Cantidad),
                    AreaTotal = g.Sum(c => c.Cultivo.Terreno.Area),
                    Rendimiento = g.Sum(c => c.Cantidad) / g.Sum(c => c.Cultivo.Terreno.Area)
                })
                .ToListAsync();

            return rendimiento;
        }

        public async Task<IEnumerable<VentasMensualesDto>> GetVentasMensualesAsync(int year)
        {
            var ventas = await _context.Ventas
                .Where(v => v.FechaVenta.Year == year)
                .GroupBy(v => v.FechaVenta.Month)
                .Select(g => new VentasMensualesDto
                {
                    Mes = g.Key,
                    TotalVentas = g.Count(),
                    IngresoTotal = g.Sum(v => v.Total)
                })
                .ToListAsync();

            return ventas;
        }

        public async Task<byte[]> GenerateInformePdfAsync(int agricultorId, DateTime startDate, DateTime endDate)
        {
            // Implementación básica - en una aplicación real, usaría una librería como iTextSharp o QuestPDF
            var informe = $"Informe de Agricultor ID: {agricultorId}\n" +
                         $"Periodo: {startDate:yyyy-MM-dd} a {endDate:yyyy-MM-dd}\n" +
                         $"Generado en: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n" +
                         "Este es un informe generado automáticamente.";

            return System.Text.Encoding.UTF8.GetBytes(informe);
        }
    }
}