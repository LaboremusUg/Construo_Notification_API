namespace Construo.NotificationAPI.Models.Sms;

public class SmsServiceProvider
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Client> Clients { get; set; }
}
