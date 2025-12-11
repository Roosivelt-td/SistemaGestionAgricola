using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface ICultivoService
    {
        Task<IEnumerable<Cultivo>> GetAllCultivosAsync();
        Task<Cultivo> GetCultivoByIdAsync(int id);
        Task<Cultivo> CreateCultivoAsync(Cultivo cultivo);
        Task<Cultivo> UpdateCultivoAsync(int id, Cultivo cultivo);
        Task<bool> DeleteCultivoAsync(int id);
        Task<IEnumerable<Cultivo>> GetCultivosByAgricultorAsync(int agricultorId);
    }
}