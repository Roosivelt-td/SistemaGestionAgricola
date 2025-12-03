using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaGestionAgricola.Data;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailVerificationService> _logger;
        private readonly Random _random;

        public EmailVerificationService(
            AppDbContext context, 
            IEmailService emailService,
            ILogger<EmailVerificationService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
            _random = new Random();
        }

        public async Task<string> GenerateAndSendVerificationCodeAsync(string email, string verificationType = "register")
        {
            try
            {
                // Limpiar verificaciones anteriores
                await CleanupOldVerificationsAsync(email);

                // Generar código de 6 dígitos
                var code = _random.Next(100000, 999999).ToString();

                // Crear registro
                var verification = new EmailVerification
                {
                    Email = email,
                    Code = code,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    VerificationType = verificationType,
                    IsUsed = false,
                    Attempts = 0
                };

                _context.EmailVerifications.Add(verification);
                await _context.SaveChangesAsync();

                // Enviar email
                var emailSent = await _emailService.SendVerificationCodeAsync(email, code);

                if (!emailSent)
                {
                    throw new Exception("No se pudo enviar el email");
                }

                _logger.LogInformation($"Código de verificación generado para {email}: {code}");
                return code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generando código de verificación para {email}");
                throw;
            }
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            try
            {
                // Buscar código válido
                var verification = await _context.EmailVerifications
                    .Where(v => v.Email == email && 
                           v.Code == code && 
                           !v.IsUsed && 
                           v.ExpiresAt > DateTime.UtcNow)
                    .FirstOrDefaultAsync();

                if (verification == null)
                {
                    // Incrementar intentos fallidos si existe
                    var existingVerification = await _context.EmailVerifications
                        .Where(v => v.Email == email && 
                               !v.IsUsed && 
                               v.ExpiresAt > DateTime.UtcNow)
                        .FirstOrDefaultAsync();

                    if (existingVerification != null)
                    {
                        existingVerification.Attempts++;
                        if (existingVerification.Attempts >= 3)
                        {
                            existingVerification.IsUsed = true;
                        }
                        await _context.SaveChangesAsync();
                    }

                    return false;
                }

                // Marcar como usado
                verification.IsUsed = true;
                verification.ExpiresAt = DateTime.UtcNow; // Expirar inmediatamente
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Código verificado correctamente para {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verificando código para {email}");
                return false;
            }
        }

        public async Task<bool> ResendVerificationCodeAsync(string email)
        {
            try
            {
                // Invalidar códigos anteriores
                await CleanupOldVerificationsAsync(email);

                // Generar y enviar nuevo código
                var code = await GenerateAndSendVerificationCodeAsync(email, "resend");
                
                return !string.IsNullOrEmpty(code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reenviando código para {email}");
                return false;
            }
        }

        public async Task CleanupExpiredVerificationsAsync()
        {
            try
            {
                // Eliminar verificaciones expiradas (más de 24 horas)
                var cutoff = DateTime.UtcNow.AddHours(-24);
                
                var expired = await _context.EmailVerifications
                    .Where(v => v.ExpiresAt < cutoff)
                    .ToListAsync();

                _context.EmailVerifications.RemoveRange(expired);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Limpieza de verificaciones expiradas: {expired.Count} eliminadas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en limpieza de verificaciones expiradas");
            }
        }

        private async Task CleanupOldVerificationsAsync(string email)
        {
            try
            {
                // Marcar como usadas todas las verificaciones anteriores para este email
                var oldVerifications = await _context.EmailVerifications
                    .Where(v => v.Email == email && !v.IsUsed)
                    .ToListAsync();

                foreach (var verification in oldVerifications)
                {
                    verification.IsUsed = true;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error limpiando verificaciones antiguas para {email}");
            }
        }
    }
}