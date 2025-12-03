namespace SistemaGestionAgricola.Services
{
    public interface IEmailVerificationService
    {
        Task<string> GenerateAndSendVerificationCodeAsync(string email, string verificationType = "register");
        Task<bool> VerifyCodeAsync(string email, string code);
        Task<bool> ResendVerificationCodeAsync(string email);
        Task CleanupExpiredVerificationsAsync();
    }
}