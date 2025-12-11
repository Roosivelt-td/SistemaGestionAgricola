using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.DTOs.Agricultor
{
    public class CreateAgricultorDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string DocumentoIdentidad { get; set; } = string.Empty;
        
        [Phone]
        public string Telefono { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;
        
        [Required]
        public int UsuarioId { get; set; }
    }
}