namespace SistemaGestionAgricola.Models.DTOs.Reportes
{
    public class VentasMensualesDto
    {
        public int Mes { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int NumeroVentas { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPromedio { get; set; }
        public decimal PorcentajeCrecimiento { get; set; }
    }
}