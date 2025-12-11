namespace SistemaGestionAgricola.Helpers
{
    public static class FechaHelper
    {
        public static bool EsEpocaSiembra(DateTime fecha, string tipoCultivo)
        {
            // Determinar la época de siembra según el tipo de cultivo
            var mes = fecha.Month;
            
            return tipoCultivo.ToLower() switch
            {
                "maiz" or "corn" => mes >= 3 && mes <= 6,  // Primavera
                "trigo" or "wheat" => mes >= 10 || mes <= 2,  // Otoño-Invierno
                "arroz" or "rice" => mes >= 4 && mes <= 7,  // Primavera-Verano
                "soya" or "soybean" => mes >= 4 && mes <= 6,  // Primavera
                "frijol" or "beans" => mes >= 5 && mes <= 8,  // Verano
                _ => true  // Por defecto, permitir siembra
            };
        }

        public static bool EsEpocaCosecha(DateTime fecha, string tipoCultivo)
        {
            var mes = fecha.Month;
            
            return tipoCultivo.ToLower() switch
            {
                "maiz" or "corn" => mes >= 8 && mes <= 11,  // Otoño
                "trigo" or "wheat" => mes >= 4 && mes <= 7,  // Primavera-Verano
                "arroz" or "rice" => mes >= 8 && mes <= 10,  // Otoño
                "soya" or "soybean" => mes >= 9 && mes <= 11,  // Otoño
                "frijol" or "beans" => mes >= 9 && mes <= 11,  // Otoño
                _ => true  // Por defecto, permitir cosecha
            };
        }

        public static int CalcularSemanaAgricola(DateTime fecha)
        {
            var primerDiaAnio = new DateTime(fecha.Year, 1, 1);
            var diasDesdeInicio = (fecha - primerDiaAnio).Days;
            return (int)Math.Floor(diasDesdeInicio / 7.0) + 1;
        }

        public static DateTime ObtenerFechaInicioEstacion(string estacion, int anio)
        {
            return estacion.ToLower() switch
            {
                "primavera" => new DateTime(anio, 3, 20),
                "verano" => new DateTime(anio, 6, 21),
                "otoño" => new DateTime(anio, 9, 22),
                "invierno" => new DateTime(anio, 12, 21),
                _ => DateTime.Now
            };
        }
    }
}