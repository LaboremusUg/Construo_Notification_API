using Construo.NotificationAPI.Core.Exceptions;
using Construo.NotificationAPI.Core.Extensions;
using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Repository;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Construo.NotificationAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly IMessageLoggingService _messageLoggingService;
    private readonly IExchangeMailService _exchangeMailService;

    /// <inheritdoc />
    public EmailService(IHttpContextAccessor httpContext,
        Microsoft.Extensions.Hosting.IHostingEnvironment environment,
        IConfiguration configuration,
        ILogger<EmailService> logger,
        IMessageQueueRepository messageQueueRepository, IMessageLoggingService messageLoggingService, IExchangeMailService exchangeMailService)
    {
        _configuration = configuration;
        _logger = logger;
        _messageLoggingService = messageLoggingService;
        _exchangeMailService = exchangeMailService;
    }

    /// <inheritdoc />
    public async Task<MessageServiceResult> SendMailAsync(EmailDetails email, Guid id)
    {


        var emailConfig = _configuration.GetEmailSettings();
        //_logger.LogInformation("Initializing memory streams..");
        var streams = new List<MemoryStream>();
        var senderEmail = email.Message.From.Or(emailConfig.From);
        var senderDisplay = email.Message.SenderName.Or(emailConfig.DisplayName);
        if (string.IsNullOrWhiteSpace(senderEmail))
            throw new ClientFriendlyException("Invalid sender");

        var message = new MailMessage
        {
            Subject = email.Message.Subject,
            Body = email.Message.Body,
            From = new MailAddress(senderEmail, senderDisplay),
            IsBodyHtml = email.Message.IsBodyHtml,
        };


        var recipients = _configuration.GetOverridingRecipients();
        if (recipients != null && recipients.Any())
        {
            _logger.LogInformation($"Overriding recipients: {string.Join(",", recipients)}");
            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();

            foreach (var recipient in recipients)
            {
                message.To.Add(new MailAddress(recipient));
            }
        }
        else
        {
            _logger.LogInformation($"Adding recipients");
            if (email.Message.To == null || !email.Message.To.Any())
            {
                _logger.LogError("Email recipient was not found.");
                throw new ArgumentNullException(nameof(email.Message.To));
            }

            foreach (var to in email.Message.To)
            {
                if (string.IsNullOrEmpty(to))
                {
                    return new MessageServiceResult(new List<string> { "Primary email recipient is missing" });
                }
                message.To.Add(new MailAddress(to));
            }

            if (email.Message.Cc != null && email.Message.Cc.Any())
            {
                foreach (var cc in email.Message.Cc)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            if (email.Message.Bcc != null && email.Message.Bcc.Any())
            {
                foreach (var bcc in email.Message.Bcc)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }

            if (email.Message.Attachments != null && email.Message.Attachments.Any())
            {
                foreach (var attachment in email.Message.Attachments)
                {
                    var stream = new MemoryStream(attachment.Bytes);

                    stream.Seek(0, SeekOrigin.Begin);
                    var fileAttachment = new Attachment(stream, attachment.FileName)
                    {
                        ContentType =
                            {
                                MediaType = MediaTypeNames.Application.Pdf
                            }
                    };

                    message.Attachments.Add(fileAttachment);
                    streams.Add(stream);
                }
            }
        }

        _logger.LogInformation("Sending email...");
        var result = await SendMailNowAsync(message);

        // Get message queue item by Id
        var messageQueueItem = await _messageLoggingService.GetByIdAsync(id);

        // If exists, update with status
        if (messageQueueItem != null)
        {
            if (result.Succeeded)
            {
                messageQueueItem.Status = SendStatus.Sent;
                messageQueueItem.OverriddenRecipients = recipients.CommaSeparated();
                messageQueueItem.TimeSentAttempt = DateTime.Now;
            }
            else
            {
                messageQueueItem.Status = SendStatus.Failed;
            }

            messageQueueItem.SendAttempts += 1;
            await _messageLoggingService.UpdateAsync(messageQueueItem);
        }
        else
        {
            await _messageLoggingService.CreateAsync(email.MessageQueueItem());
        }
        // else create new entry

        if (!result.Succeeded)
        {
            throw new Exception("Sending email failed: " + string.Join(",", result.Errors));
        }

        if (message.Attachments.Any())
        {
            _logger.LogInformation("Disposing memory streams...");
            foreach (var stream in streams)
            {
                stream.Dispose();
            }
        }

        if (result.Succeeded)
        {
            _logger.LogInformation("Email was sent successfully.");
        }
        else
        {
            var errors = string.Join(", ", result.Errors);

            _logger.LogError($"Error while sending email: {errors}");
            throw new Exception("Error while sending email: " + errors);
        }

        return result;
    }


    #region Private

    private async Task<MessageServiceResult> SendMailNowAsync(MailMessage message)
    {
        try
        {
            var sender = _configuration.GetConfiguredMailSender();
            if (sender == MailSender.ExchangeServer)
            {
                _logger.LogInformation("Sending email via ExchangeServer...");
                _exchangeMailService.SendEmail(message);
            }
            else
            {
                _logger.LogInformation("Sending email via SMTP...");
                await SendMailImmediatelyAndWait(message);
            }
            return MessageServiceResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occurred when sending mail: {0}. \n{1}", e, e.Message);
            var sendStatusComment = e.ToString();

            return MessageServiceResult.Failed(e.ToString());
        }
    }

    private async Task SendMailImmediatelyAndWait(MailMessage message)
    {
        using (var smtpClient = new SmtpClient())
        {
            var settings = _configuration.GetEmailSettings();

            smtpClient.Host = settings.Host;
            smtpClient.DeliveryMethod = settings.DeliveryMethod;

            smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);
            smtpClient.Port = settings.Port;
            smtpClient.EnableSsl = settings.EnableSsl;

            await smtpClient.SendMailAsync(message);
        }
    }

    #endregion Private
}
