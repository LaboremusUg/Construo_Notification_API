namespace Construo.NotificationAPI.Models.Sms;

public class VianettSms
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Msgid { get; set; }
    public string Msg { get; set; }
    public string CampaignId { get; set; }
    public string SenderAddress { get; set; }
    public string Tel { get; set; }
    public SenderAddressType SenderAddressType { get; set; } = SenderAddressType.AlphaNumeric;

}

public enum SenderAddressType
{
    MSISN = 1,
    ShortCode = 2,
    AlphaNumeric = 5
}
