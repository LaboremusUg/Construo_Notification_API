namespace Construo.NotificationAPI.Models;

public enum SendStatus
{
    ReadyToSend = 1,
    TrySending = 2,
    Failed = 3,
    FailedPermanently = 4,
    Sent = 5
}

public enum SendOption
{
    Default = 1,
    AddToQueue = 2,
    SendImidiatelyAndWait = 3,
    SendImidiatelyAndDoNotWait = 4
}

public enum MessageType
{
    Email = 0,
    Sms = 1
}

public enum EmailGroup
{
    Default = 0,
    Applicant = 1,
    CoApplicant = 2,
    General = 6
}

/// <summary>
/// This holds the list of all supported SMS service providers. The list is persisted to the database.
/// </summary>
public enum SmsServiceProviderType
{
    Twilio = 1,
    YoUganda = 2,
    Dfcu = 3,
    ViaNett = 4
}