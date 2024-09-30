using System.ComponentModel.DataAnnotations;

namespace Construo.NotificationAPI.Models.Sms;

public class Client
{
    public Guid Id { get; set; }
    /// <summary>
    /// Name of  the client
    /// </summary>
    public string Name { get; set; }
    public int ServiceProviderId { get; set; }
    /// <summary>
    /// The user name provided by the service provider to authenticate
    /// </summary>
    public string ServiceProviderUsername { get; set; }
    /// <summary>
    /// The password provided by the service provider to authenticate
    /// </summary>
    public string ServiceProviderPassword { get; set; }
    public virtual SmsServiceProvider? ServiceProvider { get; set; }
    /// <summary>
    /// A preferred custom sender Id for all messages sent. It may or may not work depending on the service provider
    /// </summary>
    [RegularExpression("^\\w{3,12}$")]
    [MaxLength(12)]
    [MinLength(3)]
    public string SenderId { get; set; }
}
