using System.ComponentModel.DataAnnotations;

namespace Construo.NotificationAPI.ViewModels;

public class Sms
{
    /// <summary>
    /// The SMS content
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// A list of phone numbers to send the sms to
    /// </summary>
    public List<string> Recipients { get; set; }

    /// <summary>
    /// The identifier for the client sending the sms. For emata this should be the ParentOrganisationId
    /// </summary>
    [Required]
    public Guid ClientId { get; set; }
}
