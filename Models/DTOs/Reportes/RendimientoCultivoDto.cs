namespace SistemaGestionAgricola.Models.DTOs.Reportes
{
    public class RendimientoCultivoDto
    {
        public string NombreCultivo { get; set; } = string.Empty;
        public string TipoCultivo { get; set; } = string.Empty;
        public decimal Rendimiento { get; set; }
        public decimal ProduccionTotal { get; set; }
        public decimal AreaCultivada { get; set; }
        public string UnidadRendimiento { get; set; } = string.Empty;
        public string AgricultorNombre { get; set; } = string.Empty;
        public DateTime FechaUltimaCosecha { get; set; }
    }
}