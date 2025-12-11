namespace SistemaGestionAgricola.Models.DTOs.Cosecha
{
    public class CosechaStatsDto
    {
        public int TotalCosechas { get; set; }
        public decimal ProduccionTotal { get; set; }
        public decimal ProduccionPromedio { get; set; }
        public decimal MejorRendimiento { get; set; }
        public string MejorCultivo { get; set; } = string.Empty;
        public decimal ValorEstimado { get; set; }
    }
}