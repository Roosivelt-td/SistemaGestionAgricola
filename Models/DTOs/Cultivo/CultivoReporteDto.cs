namespace SistemaGestionAgricola.Models.DTOs.Cultivo
{
    public class CultivoReporteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoCultivo { get; set; } = string.Empty;
        public decimal AreaCultivada { get; set; }
        public DateTime FechaSiembra { get; set; }
        public DateTime? FechaCosecha { get; set; }
        public decimal Rendimiento { get; set; }
        public decimal ProduccionTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string AgricultorNombre { get; set; } = string.Empty;
    }
}