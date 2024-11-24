using API.Iserviecs;
using DBcontext.Viewmodel;
using System.Net.Mail;
using System.Net;

public class MailServices : IMailServices
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<MailServices> _logger;

    public MailServices(SmtpSettings smtpSettings, ILogger<MailServices> logger)
    {
        _smtpSettings = smtpSettings;
        _logger = logger;
    }

    public async Task<bool> SendMail(MailData mailData)
    {
        try
        {
            
            var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password)
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Email, "My Application"),
                Subject = mailData.EmailSubject,
                Body = mailData.EmailBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(mailData.EmailToId, mailData.EmailToName));

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            // Ghi log chi tiết lỗi
            _logger.LogError($"Error sending email: {ex.Message}");
            if (ex.InnerException != null)
            {
                _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
            }
            return false;
        }
    }
}
