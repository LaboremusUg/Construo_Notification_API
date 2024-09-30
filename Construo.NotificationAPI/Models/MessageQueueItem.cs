using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construo.NotificationAPI.Models;

public class MessageQueueItem
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public DateTime TimeRegistered { get; set; }
    public DateTime TimeToSend { get; set; }
    public DateTime? TimeSentAttempt { get; set; }
    public int SendAttempts { get; set; }
    public int? MaxSendAttempts { get; set; }
    public string Context { get; set; }
    public int? ContextId { get; set; }
    public int? SentByContactId { get; set; }
    public int? RecipientContactId { get; set; }
    public TemplateType? TemplateType { get; set; }
    public EmailGroup EmailGroup { get; set; }
    public string SentByContact { get; set; }
    public SendStatus Status { get; set; }
    public string StatusComment { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }
    public string OverriddenRecipients { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
    public MessageType MessageType { get; set; }
    public override string ToString()
    {
        return string.Format(
            "Id: {0} \nTimeRegistered: {1} \nTimeToSend: {2} \nTimeSentAttempt: {3} \nSendAttempts: {4} \nMaxSendAttempts: {5} \nContext: {6} \nStatus: {7} \nStatusComment: {8} \n" +
            "From: {9} \nTo: {10} \nCc: {11} \nBcc: {12} \nOverriddenRecipients: {13} \nSubject: {14} \nBody: \n{15}",
            Id,
            TimeRegistered,
            TimeToSend,
            TimeSentAttempt,
            SendAttempts,
            MaxSendAttempts,
            Context,
            Status,
            StatusComment,
            From,
            To,
            Cc,
            Bcc,
            OverriddenRecipients,
            Subject,
            Body
        );
    }
}