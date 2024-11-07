using Construo.NotificationAPI.Core.Extensions;
using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Core.Helpers;

public static class Extensions
{
    public static string ToLowerCamelCase(this string str)
    {
        return str.First().ToString().ToLower() + str.Substring(1);
    }

    public static List<string> GetOverridingRecipients(this IConfiguration configuration)
    {
        return configuration.GetSection("Messaging:Recipients:Emails").Get<List<string>>();
    }

    public static SendOption GetDefaultSendOption(this IConfiguration configuration)
    {
        return (SendOption)configuration.GetSection("Messaging:DefaultSendOption").Get<int>();
    }

    public static EmailSettings GetEmailSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Email:Smtp").Get<EmailSettings>();
    }

    public static ExchangeEmailSettings GetExchangeEmailSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Email:ExchangeServer").Get<ExchangeEmailSettings>();
    }

    public static MailSender GetConfiguredMailSender(this IConfiguration configuration)
    {
        return configuration.GetSection("Email:MailSender").Get<MailSender>();
    }

    public static string GetEmailTemplatePath(this IConfiguration configuration)
    {
        return configuration.GetSection("Messaging:Templates").Get<string>();
    }

    public static string GetSmsCountryCode(this IConfiguration configuration)
    {
        return configuration.GetSection("Messaging:CountryCode").Get<string>();
    }

    public static TwilioConfig GetTwilioConfig(this IConfiguration config)
    {
        return config.GetSection("Providers:Twilio").Get<TwilioConfig>();
    }

    public static string YoUgandaUrl(this IConfiguration configuration)
    {
        return configuration.GetSection("Providers:YoUganda:Url").Value;
    }
    public static string VianettUrl(this IConfiguration configuration)
    {
        return configuration.GetSection("Providers:Vianett:Url").Value;
    }
    public static string GetDfcuUrl(this IConfiguration configuration)
    {
        return configuration.GetSection("Providers:Dfcu:Url").Value;
    }

    public static SeqSettings GetSeqSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(SeqSettings.SectionName).Get<SeqSettings>();
    }
}