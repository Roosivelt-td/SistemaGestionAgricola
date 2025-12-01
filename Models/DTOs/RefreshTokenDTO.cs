using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
