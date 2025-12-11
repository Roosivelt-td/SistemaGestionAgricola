namespace SistemaGestionAgricola.Models.DTOs.Terrero
{
    public class TerreroDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Area { get; set; }
        public string UnidadMedidaArea { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string TipoSuelo { get; set; } = string.Empty;
        public int AgricultorId { get; set; }
        public string AgricultorNombre { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}