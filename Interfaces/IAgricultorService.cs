using SistemaGestionAgricola.Models.Entities;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Interfaces
{
    public interface IAgricultorService
    {
        Task<IEnumerable<Agricultor>> GetAllAgricultoresAsync();
        Task<Agricultor> GetAgricultorByIdAsync(int id);
        Task<Agricultor> GetAgricultorByUsuarioIdAsync(int usuarioId);
        Task<Agricultor> CreateAgricultorAsync(Agricultor agricultor);
        Task<Agricultor> UpdateAgricultorAsync(int id, Agricultor agricultor);
        Task<bool> DeleteAgricultorAsync(int id);
    }
}