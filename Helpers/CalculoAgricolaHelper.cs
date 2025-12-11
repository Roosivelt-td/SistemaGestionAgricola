namespace SistemaGestionAgricola.Helpers
{
    public static class CalculoAgricolaHelper
    {
        public static decimal CalcularRendimiento(decimal produccionTotal, decimal areaCultivada)
        {
            if (areaCultivada <= 0)
                return 0;
                
            return Math.Round(produccionTotal / areaCultivada, 2);
        }

        public static decimal CalcularEficienciaAgua(decimal volumenAguaUsado, decimal produccionTotal)
        {
            if (produccionTotal <= 0)
                return 0;
                
            return Math.Round(volumenAguaUsado / produccionTotal, 2);
        }

        public static decimal CalcularCostoProduccion(decimal costosDirectos, decimal costosIndirectos, decimal produccionTotal)
        {
            if (produccionTotal <= 0)
                return 0;
                
            return Math.Round((costosDirectos + costosIndirectos) / produccionTotal, 2);
        }

        public static int CalcularDiasCrecimiento(DateTime fechaSiembra, DateTime? fechaCosecha = null)
        {
            var fechaFinal = fechaCosecha ?? DateTime.Now;
            return (int)(fechaFinal - fechaSiembra).TotalDays;
        }

        public static decimal CalcularBeneficioNeto(decimal ingresos, decimal costos)
        {
            return Math.Round(ingresos - costos, 2);
        }

        public static decimal CalcularRentabilidad(decimal beneficioNeto, decimal inversionInicial)
        {
            if (inversionInicial <= 0)
                return 0;
                
            return Math.Round((beneficioNeto / inversionInicial) * 100, 2);
        }
    }
}