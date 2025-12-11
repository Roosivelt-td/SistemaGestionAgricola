using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.DTOs.Cultivo
{
    public class CreateCultivoDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        [Required]
        public int TipoCultivoId { get; set; }
        
        [Required]
        public decimal AreaCultivada { get; set; }
        
        [Required]
        public DateTime FechaSiembra { get; set; }
        
        public DateTime? FechaCosechaEstimada { get; set; }
        
        [Required]
        public int AgricultorId { get; set; }
        
        [Required]
        public int TerrenoId { get; set; }
    }
}