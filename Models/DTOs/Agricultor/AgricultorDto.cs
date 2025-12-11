namespace SistemaGestionAgricola.Models.DTOs.Agricultor
{
    public class AgricultorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
}