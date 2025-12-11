using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface ITerreroService
    {
        Task<IEnumerable<Terreno>> GetAllTerrenosAsync();
        Task<Terreno> GetTerrenoByIdAsync(int id);
        Task<Terreno> CreateTerrenoAsync(Terreno terreno);
        Task<Terreno> UpdateTerrenoAsync(int id, Terreno terreno);
        Task<bool> DeleteTerrenoAsync(int id);
        Task<IEnumerable<Terreno>> GetTerrenosByAgricultorAsync(int agricultorId);
    }
}