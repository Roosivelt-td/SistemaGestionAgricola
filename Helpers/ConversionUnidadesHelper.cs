namespace SistemaGestionAgricola.Helpers
{
    public static class ConversionUnidadesHelper
    {
        // Factores de conversión
        private const decimal LibrasAKg = 0.453592m;
        private const decimal OnzasAKg = 0.0283495m;
        private const decimal ToneladasAKg = 1000m;
        private const decimal HectareasAAcres = 2.47105m;
        private const decimal MetrosACentimetros = 100m;
        private const decimal LitrosAGalones = 0.264172m;

        public static decimal ConvertirKilogramos(decimal valor, string unidadOrigen)
        {
            return unidadOrigen.ToLower() switch
            {
                "kg" => valor,
                "ton" => valor * ToneladasAKg,
                "lb" => valor * LibrasAKg,
                "oz" => valor * OnzasAKg,
                _ => valor
            };
        }

        public static decimal ConvertirHectareas(decimal valor, string unidadOrigen)
        {
            return unidadOrigen.ToLower() switch
            {
                "ha" => valor,
                "acres" => valor * HectareasAAcres,
                _ => valor
            };
        }

        public static decimal ConvertirLitros(decimal valor, string unidadOrigen)
        {
            return unidadOrigen.ToLower() switch
            {
                "l" => valor,
                "gal" => valor * LitrosAGalones,
                _ => valor
            };
        }

        public static decimal ConvertirMetros(decimal valor, string unidadOrigen)
        {
            return unidadOrigen.ToLower() switch
            {
                "m" => valor,
                "cm" => valor / MetrosACentimetros,
                _ => valor
            };
        }
    }
}