// Services/CodigoVerificacionService.cs
namespace SistemaGestionAgricola.Services
{
    public interface ICodigoVerificacionService
    {
        string GenerarCodigo6Digitos();
        bool ValidarCodigo(string codigoAlmacenado, string codigoIngresado, DateTime? expiracion);
    }

    public class CodigoVerificacionService : ICodigoVerificacionService
    {
        private readonly Random _random = new Random();

        public string GenerarCodigo6Digitos()
        {
            // Generar código de 6 dígitos numéricos
            return _random.Next(100000, 999999).ToString();
        }

        public bool ValidarCodigo(string codigoAlmacenado, string codigoIngresado, DateTime? expiracion)
        {
            // Verificar que el código exista
            if (string.IsNullOrEmpty(codigoAlmacenado) || string.IsNullOrEmpty(codigoIngresado))
                return false;

            // Verificar que no haya expirado
            if (expiracion.HasValue && expiracion.Value < DateTime.UtcNow)
                return false;

            // Comparar códigos (case insensitive)
            return codigoAlmacenado.Trim().Equals(codigoIngresado.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}