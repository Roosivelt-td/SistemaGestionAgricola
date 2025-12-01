using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAgricola.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Rol { get; set; } = "pendiente";

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = "Pendiente";
        
        [MaxLength(100)]
        public string? Apellidos { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }

        // ✅ NUEVOS CAMPOS PARA VERIFICACIÓN POR EMAIL
        public bool EmailVerificado { get; set; } = false;
        public string? CodigoVerificacion { get; set; }
        public DateTime? CodigoVerificacionExpiracion { get; set; }
        public DateTime? FechaVerificacion { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property para Agricultor 
        public virtual Agricultor? Agricultor { get; set; }
        public virtual ICollection<NotificacionAutomatica> NotificacionesAutomaticas { get; set; } = new List<NotificacionAutomatica>();
        public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
        
        public Usuario()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}