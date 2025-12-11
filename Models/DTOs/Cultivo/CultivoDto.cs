namespace SistemaGestionAgricola.Models.DTOs.Cultivo
{
    public class CultivoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int TipoCultivoId { get; set; }
        public string TipoCultivoNombre { get; set; } = string.Empty;
        public decimal AreaCultivada { get; set; }
        public DateTime FechaSiembra { get; set; }
        public DateTime? FechaCosechaEstimada { get; set; }
        public int AgricultorId { get; set; }
        public string AgricultorNombre { get; set; } = string.Empty;
        public int TerrenoId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}