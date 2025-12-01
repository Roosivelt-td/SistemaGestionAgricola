// Services/EmailService.cs
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SistemaGestionAgricola.Models.DTOs;

namespace SistemaGestionAgricola.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarEmailAsync(EmailRequestDTO emailRequest);
        Task<bool> EnviarCodigoVerificacionAsync(string email, string codigo);
        Task EnviarBienvenidaAgricultorAsync(string email, string nombre);
        Task EnviarNotificacionCosechaAsync(string email, string cultivo, DateTime fechaEstimada);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> EnviarEmailAsync(EmailRequestDTO emailRequest)
        {
            try
            {
                _logger.LogInformation($"📧 Enviando email a: {emailRequest.Para}");
                
                var config = ObtenerConfiguracionEmail();
                
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(config.DeNombre, config.DeEmail));
                mensaje.To.Add(MailboxAddress.Parse(emailRequest.Para));
                mensaje.Subject = emailRequest.Asunto;

                var bodyBuilder = new BodyBuilder();
                if (emailRequest.EsHtml)
                {
                    bodyBuilder.HtmlBody = emailRequest.Cuerpo;
                }
                else
                {
                    bodyBuilder.TextBody = emailRequest.Cuerpo;
                }

                mensaje.Body = bodyBuilder.ToMessageBody();

                using var clienteSmtp = new SmtpClient();
                await clienteSmtp.ConnectAsync(config.SmtpServer, config.SmtpPort, 
                    config.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                
                await clienteSmtp.AuthenticateAsync(config.Usuario, config.Password);
                await clienteSmtp.SendAsync(mensaje);
                await clienteSmtp.DisconnectAsync(true);

                _logger.LogInformation($"✅ Email enviado exitosamente a: {emailRequest.Para}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar email a: {emailRequest.Para}");
                return false;
            }
        }

        public async Task<bool> EnviarCodigoVerificacionAsync(string email, string codigo)
        {
            var htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                    .codigo {{ 
                        background-color: #f0f0f0; 
                        padding: 20px; 
                        text-align: center; 
                        font-size: 36px; 
                        font-weight: bold; 
                        letter-spacing: 10px; 
                        margin: 30px 0; 
                        border-radius: 10px;
                        color: #333;
                        border: 2px dashed #4CAF50;
                    }}
                    .info {{ background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                    .footer {{ color: #777; font-size: 12px; margin-top: 30px; text-align: center; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🔐 Código de Verificación</h1>
                    </div>
                    
                    <h2>¡Hola!</h2>
                    <p>Para completar tu registro en <strong>Sistema Gestión Agrícola</strong>, 
                       utiliza el siguiente código:</p>
                    
                    <div class='codigo'>{codigo}</div>
                    
                    <div class='info'>
                        <p><strong>⚠️ Importante:</strong></p>
                        <ul>
                            <li>Este código expira en <strong>15 minutos</strong></li>
                            <li>No lo compartas con nadie</li>
                            <li>Si no solicitaste este registro, ignora este email</li>
                        </ul>
                    </div>
                    
                    <div class='footer'>
                        <p>© 2024 Sistema Gestión Agrícola</p>
                        <p>Email automático - No responder</p>
                    </div>
                </div>
            </body>
            </html>";

            var emailRequest = new EmailRequestDTO
            {
                Para = email,
                Asunto = "🔐 Tu código de verificación - Sistema Agrícola",
                Cuerpo = htmlContent,
                EsHtml = true
            };

            return await EnviarEmailAsync(emailRequest);
        }

        public async Task EnviarBienvenidaAgricultorAsync(string email, string nombre)
        {
            var htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; background-color: #f9f9f9; }}
                    .footer {{ background-color: #333; color: white; padding: 10px; text-align: center; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🌱 ¡Bienvenido a Sistema Gestión Agrícola!</h1>
                    </div>
                    <div class='content'>
                        <h2>¡Hola {nombre}!</h2>
                        <p>Tu registro como agricultor se ha completado exitosamente.</p>
                        <p>Ahora puedes:</p>
                        <ul>
                            <li>🌾 Gestionar tus cultivos</li>
                            <li>📅 Programar procesos agrícolas</li>
                            <li>💰 Registrar cosechas y ventas</li>
                            <li>🔔 Recibir notificaciones importantes</li>
                        </ul>
                        <p>¡Comienza a optimizar tu producción agrícola hoy mismo!</p>
                        <p>Saludos,<br>El equipo de Sistema Gestión Agrícola</p>
                    </div>
                    <div class='footer'>
                        <p>© 2024 Sistema Gestión Agrícola</p>
                    </div>
                </div>
            </body>
            </html>";

            var emailRequest = new EmailRequestDTO
            {
                Para = email,
                Asunto = "🌱 ¡Bienvenido a Sistema Gestión Agrícola!",
                Cuerpo = htmlContent,
                EsHtml = true
            };

            await EnviarEmailAsync(emailRequest);
        }

        public async Task EnviarNotificacionCosechaAsync(string email, string cultivo, DateTime fechaEstimada)
        {
            var htmlContent = $@"
            <div style='font-family: Arial, sans-serif;'>
                <h2 style='color: #4CAF50;'>📅 Recordatorio de Cosecha</h2>
                <p><strong>Cultivo:</strong> {cultivo}</p>
                <p><strong>Fecha estimada:</strong> {fechaEstimada:dd/MM/yyyy}</p>
                <p>¡Prepárate para la cosecha!</p>
            </div>";

            var emailRequest = new EmailRequestDTO
            {
                Para = email,
                Asunto = $"📅 Recordatorio: Cosecha de {cultivo}",
                Cuerpo = htmlContent,
                EsHtml = true
            };

            await EnviarEmailAsync(emailRequest);
        }

        private EmailConfigDTO ObtenerConfiguracionEmail()
        {
            return new EmailConfigDTO
            {
                SmtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com",
                SmtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
                Usuario = _configuration["Email:Usuario"] ?? "",
                Password = _configuration["Email:Password"] ?? "",
                DeNombre = _configuration["Email:DeNombre"] ?? "Sistema Gestión Agrícola",
                DeEmail = _configuration["Email:DeEmail"] ?? "",
                UseSsl = bool.Parse(_configuration["Email:UseSsl"] ?? "true")
            };
        }
    }
}