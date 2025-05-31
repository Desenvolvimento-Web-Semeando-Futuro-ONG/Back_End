using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Back_End.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"]!;
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]!);
            _smtpUsername = configuration["EmailSettings:SmtpUsername"]!;
            _smtpPassword = configuration["EmailSettings:SmtpPassword"]!;
            _enableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"]!);
            _fromEmail = configuration["EmailSettings:FromEmail"]!;
        }

        public async Task EnviarEmailAsync(string email, string assunto, string mensagem)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = _enableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
} 