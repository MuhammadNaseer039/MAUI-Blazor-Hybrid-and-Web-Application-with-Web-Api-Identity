using System.Net;
using System.Net.Mail;

namespace UserManagemanetAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            
            string? MailServer = configuration["EmailSettings:MailServer"];
            
            string? FromEmail = configuration["EmailSettings:FromEmail"];
            
            string? Password = configuration["EmailSettings:Password"];
            
            string? SenderName = configuration["EmailSettings:SenderName"];
            
            int Port = Convert.ToInt32(configuration["EmailSettings:MailPort"]);
            
            var client = new SmtpClient(MailServer, Port)
            {
                
                Credentials = new NetworkCredential(FromEmail, Password),
                
                EnableSsl = true,
            };
            
            MailAddress fromAddress = new MailAddress(FromEmail, SenderName);
            
            MailMessage mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = Subject,
                Body = Body,
                IsBodyHtml = IsBodyHtml
            };
            
            mailMessage.To.Add(ToEmail);
            
            return client.SendMailAsync(mailMessage);
        }
    }
}
