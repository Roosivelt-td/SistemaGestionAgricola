using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.DTOs.Terrero
{
    public class CreateTerreroDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        [Required]
        public decimal Area { get; set; }
        
        [Required]
        [StringLength(10)]
        public string UnidadMedidaArea { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Ubicacion { get; set; } = string.Empty;
        
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        
        [StringLength(50)]
        public string TipoSuelo { get; set; } = string.Empty;
        
        [Required]
        public int AgricultorId { get; set; }
    }
}