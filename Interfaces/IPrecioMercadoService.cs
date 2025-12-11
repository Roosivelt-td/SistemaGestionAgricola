namespace SistemaGestionAgricola.Interfaces
{
    public interface IPrecioMercadoService
    {
        Task<decimal> GetPrecioProductoAsync(string producto, string ubicacion);
        Task<IEnumerable<dynamic>> GetPreciosProductosAsync(IEnumerable<string> productos, string ubicacion);
        Task<bool> AlertaBajoPrecioAsync(string producto, decimal precioMinimo);
    }
}