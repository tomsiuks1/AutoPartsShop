using System.Net.Mail;
using System.Net;
using API.Interfaces;

namespace API.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(string smtpServer, int port, string fromEmail, string password)
        {
            _smtpServer = smtpServer;
            _port = port;
            _fromEmail = fromEmail;
            _password = password;
        }

        public async Task SendEmail(string toEmail, string body)
        {
            using (var smtpClient = new SmtpClient(_smtpServer))
            {
                smtpClient.Port = _port;
                smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = "Order confirmation",
                    Body = body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}