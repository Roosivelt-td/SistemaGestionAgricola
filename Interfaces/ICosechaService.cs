using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface ICosechaService
    {
        Task<IEnumerable<Cosecha>> GetAllCosechasAsync();
        Task<Cosecha> GetCosechaByIdAsync(int id);
        Task<Cosecha> CreateCosechaAsync(Cosecha cosecha);
        Task<Cosecha> UpdateCosechaAsync(int id, Cosecha cosecha);
        Task<bool> DeleteCosechaAsync(int id);
        Task<IEnumerable<Cosecha>> GetCosechasByCultivoAsync(int cultivoId);
        Task<IEnumerable<Cosecha>> GetCosechasByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}