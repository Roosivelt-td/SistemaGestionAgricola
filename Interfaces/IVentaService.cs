using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> GetAllVentasAsync();
        Task<Venta> GetVentaByIdAsync(int id);
        Task<Venta> CreateVentaAsync(Venta venta);
        Task<Venta> UpdateVentaAsync(int id, Venta venta);
        Task<bool> DeleteVentaAsync(int id);
        Task<IEnumerable<Venta>> GetVentasByAgricultorAsync(int agricultorId);
        Task<IEnumerable<Venta>> GetVentasByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}