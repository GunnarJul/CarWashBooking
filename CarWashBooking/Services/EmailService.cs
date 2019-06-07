using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Services
{
    public class EmailService : IEmailService
    {
        private EmailServiceOptions _emailServiceOptions;
        readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailServiceOptions> emailServiceOptions,
            ILogger<EmailService> logger)
        {
            _emailServiceOptions = emailServiceOptions.Value;
            _logger = logger;
        }


        public Task SendEmail(string emailFrom, string emailTo, string subject, string message)
        {
            try
            {
                using (var client = new SmtpClient(_emailServiceOptions.MailServer, int.Parse(_emailServiceOptions.MailPort)))
                {
                    if (bool.Parse(_emailServiceOptions.UseSSL) == true)
                        client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(_emailServiceOptions.UserId))
                        client.Credentials = new NetworkCredential(_emailServiceOptions.UserId, _emailServiceOptions.Password);
                    client.Send(new MailMessage(emailFrom, emailTo, subject, message));
                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Mail kan ikke sendes til {emailTo}{ex.ToString()} ");
            }
            return Task.CompletedTask;
        }
    }
}
