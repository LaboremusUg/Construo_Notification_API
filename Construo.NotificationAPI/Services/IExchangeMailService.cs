using Construo.NotificationAPI.Core.Helpers;
using Microsoft.Exchange.WebServices.Data;
using System.Net.Mail;

namespace Construo.NotificationAPI.Services;

public interface IExchangeMailService
{
    bool SendEmail(MailMessage smtpEmail, bool throwError = false);
}


/// <summary>
/// Exchange mail sender
/// </summary>
public class ExchangeMailService : IExchangeMailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public ExchangeMailService(IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public bool SendEmail(MailMessage smtpEmail, bool throwError = false)
    {
        var settings = _configuration.GetExchangeEmailSettings();
        try
        {
            var ews = new ExchangeService(ExchangeVersion.Exchange2013_SP1)
            {
                Credentials = new WebCredentials(settings.AccountName, settings.Password, settings.Domain),
                Url = new Uri(settings.Url)
            };

            var message = new EmailMessage(ews)
            {
                From = new EmailAddress(settings.From),
                Subject = smtpEmail.Subject,
                Body = smtpEmail.Body
            };
            //to recipients...                 
            message.ToRecipients.AddRange(smtpEmail.To.Select(it => new EmailAddress(it.Address)));
            //attachments...  
            foreach (var attachment in smtpEmail.Attachments)
            {
                message.Attachments.AddFileAttachment(attachment.Name, attachment.ContentStream);
            }
            message.Send();
            _logger.LogTrace("Sent Email successfully", new
            {
                SentVia = "EWS",
                message.Subject,
                RecipientCount = message.ToRecipients.Count,
                HasAttachments = message.Attachments != null && message.Attachments.Any(),
                AttachmentCount = message.Attachments != null && message.Attachments.Any() ? message.Attachments.Count : 0
            }); return true;
        }
        catch (Exception e)
        {                 // throw error if specified...
            if (throwError)
            {
                _logger.LogError("Failed to Send email", e, new
                {
                    SentVia = "EWS",
                    smtpEmail.Subject,
                    RecipientCount = smtpEmail.To.Count,
                    HasAttachments = smtpEmail.Attachments.Any(),
                    AttachmentCount = smtpEmail.Attachments.Count
                });
                throw;
            }
            return false;
        }
    }
}