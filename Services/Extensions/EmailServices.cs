namespace AlexSupport.Services.Extensions
{
    using System.Net;
    using System.Net.Mail;
    using global::AlexSupport.Services.Models;
    using Microsoft.Extensions.Options;

    public class EmailServices
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailServices> _logger;

        public EmailServices(IOptions<EmailSettings> emailSettings, ILogger<EmailServices> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<int> SendEmailAsync(
            string recipientEmail,
            string subject,
            string title,
            string mainMessage,
            string verificationCode = null,
            string footerMessage = "If you didn't request this email, please ignore it.",
            bool isVerificationEmail = false)
        {
            if (string.IsNullOrEmpty(recipientEmail))
            {
                _logger.LogWarning("Recipient email is null or empty");
                return 0;
            }

            var uid = isVerificationEmail ? new Random().Next(100000, 1000000) : 0;

            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = _emailSettings.EnableSsl;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(
                        _emailSettings.Email,
                        _emailSettings.Password);

                    var from = new MailAddress(_emailSettings.Email, "AlexSupport");
                    var to = new MailAddress(recipientEmail);

                    string bodyContent = verificationCode != null
                        ? $@"<p>Your verification code is:</p>
                         <div style='text-align: center; font-size: 32px; font-weight: bold; color: #2196F3;'>{verificationCode}</div>"
                        : mainMessage;

                    var mailMessage = new MailMessage(from, to)
                    {
                        Body = $@"
                    <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                        <div style='max-width: 600px; margin: 0 auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                            <div style='background-color: #2196F3; padding: 15px; border-radius: 5px 5px 0 0; color: white;'>
                                <h2 style='text-align: center; margin: 0;'>{title}</h2>
                            </div>
                            <div style='padding: 20px;'>
                                <p style='font-size: 16px;'>Hi, <strong>{to.Address}</strong>,</p>
                                {bodyContent}
                                <p style='margin-top: 20px;'>{footerMessage}</p>
                            </div>
                            <div style='text-align: center; padding: 10px; background-color: #f8f9fa; border-radius: 0 0 5px 5px; font-size: 12px; color: #666;'>
                                © {DateTime.Now.Year} Alex Support. All rights reserved.
                            </div>
                        </div>
                    </div>",
                        BodyEncoding = System.Text.Encoding.UTF8,
                        IsBodyHtml = true,
                        Subject = subject,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    };

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent successfully to {recipientEmail}");
                    return uid;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", recipientEmail);
                return 0;
            }
        }
    }


}
