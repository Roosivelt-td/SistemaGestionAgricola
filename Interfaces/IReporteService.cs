using SistemaGestionAgricola.Models.DTOs;
using SistemaGestionAgricola.Models.DTOs.Reportes;

namespace SistemaGestionAgricola.Interfaces
{
    public interface IReporteService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<ProduccionAnualDto>> GetProduccionAnualAsync(int year);
        Task<IEnumerable<RendimientoCultivoDto>> GetRendimientoPorCultivoAsync(int agricultorId);
        Task<IEnumerable<VentasMensualesDto>> GetVentasMensualesAsync(int year);
        Task<byte[]> GenerateInformePdfAsync(int agricultorId, DateTime startDate, DateTime endDate);
    }
}