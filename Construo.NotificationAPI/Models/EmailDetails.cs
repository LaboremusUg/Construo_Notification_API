using Construo.NotificationAPI.Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Construo.NotificationAPI.Models;

public class EmailDetails
{
    /// <summary>
    /// MailMessage Object
    /// </summary>
    [Required]
    public Message Message { get; set; }

    /// <summary>
    /// SendOption
    /// </summary>
    public SendOption SendOption { get; set; }

    /// <summary>
    /// Time to send the email
    /// </summary>
    public DateTime TimeToSend { get; set; } = DateTime.Now;

    /// <summary>
    /// Sepcifies if the retry happens if the initial sending fails
    /// </summary>
    public bool RetryIfSendFails { get; set; }

    /// <summary>
    /// Track email object
    /// </summary>
    public TrackMessageObject? TrackMail { get; set; }

    /// <summary>
    /// Email template type
    /// </summary>
    public TemplateType? TemplateType { get; set; }

    /// <summary>
    /// Model bound to the template
    /// </summary>
    public object? Model { get; set; }

    public MessageQueueItem MessageQueueItem()
    {
        return new MessageQueueItem
        {
            To = Message?.To.CommaSeparated(),
            Cc = Message?.Cc.CommaSeparated(),
            Bcc = Message?.Bcc.CommaSeparated(),
            Body = Message?.Body,
            IsBodyHtml = Message?.IsBodyHtml ?? true,
            From = Message?.From,
            MessageType = MessageType.Email,
            SendAttempts = 0,
            Status = SendStatus.ReadyToSend,
            Subject = Message?.Subject,
            TimeToSend = TimeToSend,
            TimeRegistered = DateTime.Now
        };
    }
}

/// <summary>
/// Email message details
/// </summary>
public class Message
{
    /// <summary>
    /// From
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// To
    /// </summary>
    public List<string> To { get; set; }

    /// <summary>
    /// BCC
    /// </summary>
    public List<string> Bcc { get; set; }

    /// <summary>
    /// CC
    /// </summary>
    public List<string> Cc { get; set; }

    /// <summary>
    /// Body
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Body { get; set; }

    /// <summary>
    /// Sender name
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Subject { get; set; }

    /// <summary>
    /// Is Html Body
    /// </summary>
    public bool IsBodyHtml { get; set; } = true;

    /// <summary>
    /// Attachments
    /// </summary>
    public List<EmailAttachment>? Attachments { get; set; }
}

public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] Bytes { get; set; }
}

/// <summary>
/// Email settings
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Server
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// From
    /// </summary>
    public string From { get; set; }

    public string DisplayName { get; set; }

    /// <summary>
    /// Enable SSL
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Delivery Method
    /// </summary>
    public SmtpDeliveryMethod DeliveryMethod { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }
}



/// <summary>
/// ExchangeEmail settings
/// </summary>
public class ExchangeEmailSettings
{
    /// <summary>
    /// Server
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public string AccountName { get; set; }
    /// <summary>
    /// Port
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// </summary>
    public string Domain { get; set; }



    public string From { get; set; }

    /// <summary>
    /// </summary>
    public string Version { get; set; }

}


public enum MailSender
{
    Smtp = 0,
    ExchangeServer = 1,
}