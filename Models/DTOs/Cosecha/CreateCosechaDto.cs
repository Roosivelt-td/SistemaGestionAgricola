using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.DTOs.Cosecha
{
    public class CreateCosechaDto
    {
        [Required]
        public int CultivoId { get; set; }
        
        [Required]
        public decimal CantidadCosechada { get; set; }
        
        [Required]
        [StringLength(10)]
        public string UnidadMedida { get; set; } = string.Empty;
        
        [Required]
        public DateTime FechaCosecha { get; set; }
        
        public decimal CalidadPromedio { get; set; }
        
        [StringLength(500)]
        public string Observaciones { get; set; } = string.Empty;
    }
}