namespace SistemaGestionAgricola.Models.DTOs.Reportes
{
    public class DashboardStatsDto
    {
        public int TotalCultivos { get; set; }
        public int TotalCosechas { get; set; }
        public int TotalTerrenos { get; set; }
        public int TotalAgricultores { get; set; }
        public decimal ProduccionActual { get; set; }
        public decimal ProduccionAnterior { get; set; }
        public decimal VariacionPorcentual { get; set; }
        public List<VentasMensualesDto> VentasRecientes { get; set; } = new();
        public List<RendimientoCultivoDto> MejoresRendimientos { get; set; } = new();
    }
}