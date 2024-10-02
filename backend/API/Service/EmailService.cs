using System.Net.Mail;
using System.Net;

namespace API.Service
{
    public static class EmailService
    {
        public static async Task SendEmail(string toEmail, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("tomsiuks4@gmail.com", "wtmp ufdk atrl doxl");
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("tomsiuks4@gmail.com"),
                    Subject = $"Order confirmation",
                    Body = body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
