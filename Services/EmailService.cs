using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SistemaGestionAgricola.Models.Configurations;

namespace SistemaGestionAgricola.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendVerificationCodeAsync(string toEmail, string code)
        {
            try
            {
                var subject = "Verifica tu cuenta - Sistema Gestión Agrícola";
                var body = BuildVerificationEmailBody(code);

                return await SendEmailAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando código de verificación a {toEmail}");
                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string toEmail, string userName)
        {
            try
            {
                var subject = "¡Bienvenido al Sistema de Gestión Agrícola!";
                var body = BuildWelcomeEmailBody(userName);

                return await SendEmailAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email de bienvenida a {toEmail}");
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken)
        {
            try
            {
                var subject = "Restablecer tu contraseña - Sistema Gestión Agrícola";
                var body = BuildPasswordResetEmailBody(resetToken);

                return await SendEmailAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email de restablecimiento a {toEmail}");
                return false;
            }
        }

        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(
                    _emailSettings.Username, 
                    _emailSettings.Password
                ),
                EnableSsl = _emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000 // 10 segundos timeout
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    _emailSettings.FromEmail, 
                    _emailSettings.FromName
                ),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.Normal
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
            
            _logger.LogInformation($"Email enviado a {toEmail} - Asunto: {subject}");
            return true;
        }

        private string BuildVerificationEmailBody(string code)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{ font-family: 'Arial', sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; border-radius: 10px; }}
                    .header {{ background: #2e7d32; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ padding: 30px; background: white; }}
                    .code {{ font-size: 36px; font-weight: bold; color: #2e7d32; text-align: center; 
                            padding: 20px; background: #f0f7f0; border-radius: 8px; margin: 25px 0;
                            letter-spacing: 8px; }}
                    .warning {{ background: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0; }}
                    .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🌾 Sistema de Gestión Agrícola</h1>
                    </div>
                    
                    <div class='content'>
                        <h2>Verificación de Email</h2>
                        <p>Hola,</p>
                        <p>Para completar tu registro, por favor ingresa el siguiente código:</p>
                        
                        <div class='code'>{code}</div>
                        
                        <div class='warning'>
                            <strong>⚠️ Importante:</strong>
                            <p>Este código expirará en <strong>10 minutos</strong>.</p>
                            <p>Si no solicitaste este código, ignora este email.</p>
                        </div>
                        
                        <p>Gracias por unirte a nuestro sistema.</p>
                        
                        <p>Atentamente,<br>
                        <strong>Equipo de Sistema de Gestión Agrícola</strong></p>
                    </div>
                    
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} Sistema de Gestión Agrícola. Todos los derechos reservados.</p>
                        <p>Este es un email automático, por favor no responder.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        private string BuildWelcomeEmailBody(string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>¡Bienvenido {userName}!</h2>
                <p>Tu cuenta ha sido verificada exitosamente.</p>
                <p>Ahora puedes acceder a todas las funcionalidades del sistema.</p>
            </body>
            </html>";
        }

        private string BuildPasswordResetEmailBody(string resetToken)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Restablecer Contraseña</h2>
                <p>Usa el siguiente token para restablecer tu contraseña:</p>
                <p><strong>{resetToken}</strong></p>
                <p>Este token expira en 1 hora.</p>
            </body>
            </html>";
        }
    }
}