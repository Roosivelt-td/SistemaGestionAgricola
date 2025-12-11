namespace SistemaGestionAgricola.Helpers
{
    public static class EmailHelper
    {
        public static string GetVerificationEmailTemplate(string verificationLink, string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Verificación de Email</title>
            </head>
            <body>
                <h2>Hola {userName},</h2>
                <p>Gracias por registrarte en nuestro sistema. Por favor haz clic en el siguiente enlace para verificar tu email:</p>
                <p><a href='{verificationLink}'>Verificar Email</a></p>
                <p>Si no solicitaste esta verificación, puedes ignorar este correo.</p>
            </body>
            </html>";
        }

        public static string GetPasswordResetTemplate(string resetLink, string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Restablecer Contraseña</title>
            </head>
            <body>
                <h2>Hola {userName},</h2>
                <p>Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace para continuar:</p>
                <p><a href='{resetLink}'>Restablecer Contraseña</a></p>
                <p>Si no solicitaste este cambio, puedes ignorar este correo.</p>
            </body>
            </html>";
        }
    }
}