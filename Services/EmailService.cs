using System.Net;
using System.Net.Mail;

namespace LoginAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPasswordResetEmailAsync(string email, string name, string resetLink)
        {
            try
            {
                // Para desenvolvimento, apenas log o email
                _logger.LogInformation($"Password reset email would be sent to: {email}");
                _logger.LogInformation($"Reset link: {resetLink}");

                // Em produção, implemente o envio real de email aqui
                // Exemplo com SMTP:
                
                var smtpClient = new SmtpClient(_configuration["Email:SmtpHost"])
                {
                    Port = int.Parse(_configuration["Email:SmtpPort"]),
                    Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], "Sistema de Login"),
                    Subject = "Recuperação de Senha",
                    Body = GetEmailTemplate(name, resetLink),
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset email to {Email}", email);
                throw;
            }
        }

        private string GetEmailTemplate(string name, string resetLink)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Recuperação de Senha</title>
                </head>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #4f46e5;'>Recuperação de Senha</h2>
                        <p>Olá {name},</p>
                        <p>Você solicitou a recuperação de sua senha. Clique no link abaixo para redefinir sua senha:</p>
                        <p style='margin: 30px 0;'>
                            <a href='{resetLink}' style='background-color: #4f46e5; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Redefinir Senha
                            </a>
                        </p>
                        <p><strong>Este link expira em 1 hora.</strong></p>
                        <p>Se você não solicitou esta recuperação, ignore este email.</p>
                        <hr style='margin: 30px 0;'>
                        <p style='font-size: 12px; color: #666;'>
                            Sistema de Autenticação © 2025
                        </p>
                    </div>
                </body>
                </html>";
        }
    }
}