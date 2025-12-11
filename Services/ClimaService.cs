using Microsoft.Extensions.Configuration;
using SistemaGestionAgricola.Interfaces;

namespace SistemaGestionAgricola.Services
{
    public class ClimaService : IClimaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";

        public ClimaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? string.Empty;
        }

        public async Task<dynamic> GetClimaActualAsync(double latitud, double longitud)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_baseUrl}/weather?lat={latitud}&lon={longitud}&appid={_apiKey}&units=metric&lang=es");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<dynamic>(content);
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<dynamic> GetPronosticoAsync(double latitud, double longitud)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_baseUrl}/forecast?lat={latitud}&lon={longitud}&appid={_apiKey}&units=metric&lang=es");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<dynamic>(content);
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsAdversoClimaAsync(double latitud, double longitud)
        {
            var clima = await GetClimaActualAsync(latitud, longitud);
            if (clima == null) return false;

            var condiciones = clima.GetProperty("weather")[0].GetProperty("main").ToString();
            var condicionesLower = condiciones.ToLower();
            return condicionesLower switch
            {
                "rain" => true,
                "snow" => true,
                "thunderstorm" => true,
                "drizzle" => true,
                _ => false
            };
        }
    }
}