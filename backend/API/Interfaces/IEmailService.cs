namespace API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string body);
    }
}
