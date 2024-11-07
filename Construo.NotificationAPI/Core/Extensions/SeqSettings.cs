namespace Construo.NotificationAPI.Core.Extensions;

public class SeqSettings
{
    public const string SectionName = "Seq";
    public string BaseUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
}
