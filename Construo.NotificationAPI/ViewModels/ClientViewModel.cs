using Construo.NotificationAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Construo.NotificationAPI.ViewModels;

public class ClientViewModel
{
    /// <summary>
    /// Unique identifier for the client in uuid format. If not set one will be auto-generated and returned
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Descriptive name of the client
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// The id of the service provider that this client will use to send sms
    /// </summary>
    [Required]
    public SmsServiceProviderType ServiceProviderId { get; set; }
    /// <summary>
    /// The user name provided by the service provider for authentication. If using twilio, set this to the MessagingServiceSid value
    /// </summary>
    [Required]
    public string ServiceProviderUsername { get; set; }
    /// <summary>
    /// The password provided by the service provider for authentication. This should be empty for twilio
    /// </summary>
    public string ServiceProviderPassword { get; set; }
    /// <summary>
    /// A preferred custom sender Id for all messages sent. It may or may not work depending on the service provider
    /// </summary>
    [RegularExpression("^\\w{3,12}$")]
    [MaxLength(12)]
    [MinLength(3)]
    public string SenderId { get; set; }
}
