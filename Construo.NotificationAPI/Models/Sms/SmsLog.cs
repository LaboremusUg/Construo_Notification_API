using System.ComponentModel.DataAnnotations;

namespace Construo.NotificationAPI.Models.Sms;

/// <summary>
/// Represents a record of an SMS to log to the db
/// </summary>
public class SmsLog
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The content of the SMS
    /// </summary>
    [Required]
    public string Message { get; set; }

    /// <summary>
    /// The phone number to which the sms was sent
    /// </summary>
    [Required]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Date when the SMS was sent
    /// </summary>
    [Required]
    public DateTime SendDate { get; set; }

    /// <summary>
    /// The client's unique id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// An indicator of the sms sending status. Depends on provider
    /// </summary>
    public string DeliveryStatus { get; set; }

    /// <summary>
    /// When the SMS was delivered to the intended recipient
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Name of service provider through which the sms that was sent
    /// </summary>
    public string ServiceProvider { get; set; }
}
