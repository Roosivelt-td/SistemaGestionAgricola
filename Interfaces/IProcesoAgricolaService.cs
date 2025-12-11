using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface IProcesoAgricolaService
    {
        Task<IEnumerable<ProcesoAgricola>> GetAllProcesosAsync();
        Task<ProcesoAgricola> GetProcesoByIdAsync(int id);
        Task<ProcesoAgricola> CreateProcesoAsync(ProcesoAgricola proceso);
        Task<ProcesoAgricola> UpdateProcesoAsync(int id, ProcesoAgricola proceso);
        Task<bool> DeleteProcesoAsync(int id);
        Task<IEnumerable<ProcesoAgricola>> GetProcesosByCultivoAsync(int cultivoId);
        Task<IEnumerable<ProcesoAgricola>> GetProcesosByTipoAsync(int tipoProcesoId);
    }
}