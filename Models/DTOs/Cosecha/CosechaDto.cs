namespace SistemaGestionAgricola.Models.DTOs.Cosecha
{
    public class CosechaDto
    {
        public int Id { get; set; }
        public int CultivoId { get; set; }
        public string CultivoNombre { get; set; } = string.Empty;
        public decimal CantidadCosechada { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public DateTime FechaCosecha { get; set; }
        public decimal CalidadPromedio { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}