namespace Construo.NotificationAPI.Configs;

public class EmailConfiguration
{
    public string EmailSubject { get; set; }
    public string FromEmail { get; set; }
    public string MailServer { get; set; }
    public string[] ToEmails { get; set; }

    public int Port { get; set; }
    public bool EnableSsl { get; set; }

    public string UserName { get; set; }
    public string Password { get; set; }
}
