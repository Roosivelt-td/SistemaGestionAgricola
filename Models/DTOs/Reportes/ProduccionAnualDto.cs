using SistemaGestionAgricola.Models.DTOs.Cultivo;

namespace SistemaGestionAgricola.Models.DTOs.Reportes
{
    public class ProduccionAnualDto
    {
        public int Anio { get; set; }
        public decimal ProduccionTotal { get; set; }
        public decimal IngresosTotales { get; set; }
        public int NumeroCosechas { get; set; }
        public decimal RendimientoPromedio { get; set; }
        public List<CultivoReporteDto> DetallesCultivos { get; set; } = new();
    }
}