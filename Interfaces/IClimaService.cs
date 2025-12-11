namespace SistemaGestionAgricola.Interfaces
{
    public interface IClimaService
    {
        Task<dynamic> GetClimaActualAsync(double latitud, double longitud);
        Task<dynamic> GetPronosticoAsync(double latitud, double longitud);
        Task<bool> IsAdversoClimaAsync(double latitud, double longitud);
    }
}