using SistemaGestionAgricola.Interfaces;

namespace SistemaGestionAgricola.Services
{
    public class PrecioMercadoService : IPrecioMercadoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.preciosmercado.com";

        public PrecioMercadoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["PrecioMercadoApi:ApiKey"] ?? string.Empty;
        }

        public async Task<decimal> GetPrecioProductoAsync(string producto, string ubicacion)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_baseUrl}/precio?producto={producto}&ubicacion={ubicacion}&api_key={_apiKey}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Parse the response to extract the price
                    // This is a simplified implementation
                    return 10.50m; // Placeholder value
                }
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<IEnumerable<dynamic>> GetPreciosProductosAsync(IEnumerable<string> productos, string ubicacion)
        {
            var resultados = new List<dynamic>();
            
            foreach (var producto in productos)
            {
                var precio = await GetPrecioProductoAsync(producto, ubicacion);
                resultados.Add(new { Producto = producto, Precio = precio, Ubicacion = ubicacion });
            }
            
            return resultados;
        }

        public async Task<bool> AlertaBajoPrecioAsync(string producto, decimal precioMinimo)
        {
            var precioActual = await GetPrecioProductoAsync(producto, "default");
            return precioActual < precioMinimo;
        }
    }
}