using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.ViewModels;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Construo.NotificationAPI.Services;

public class TwilioService : ISmsServiceProvider
{
    private const string MessagingServiceSid = "MG3f1f9e3b23c43d7b4ca2da2d38d94ea7";
    private readonly TwilioConfig _twilioConfig;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmsService> _logger;

    public TwilioService(IConfiguration configuration, ILogger<SmsService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _twilioConfig = configuration.GetTwilioConfig();
    }

    public async Task<SendResponse> SendAsync(List<string> phoneNumbers, string body, string username, string password = null, string senderId = null)
    {
        var serviceId = string.IsNullOrEmpty(username) ? MessagingServiceSid : username;
        int errorCount = 0;
        string failed = "";
        TwilioClient.Init(_twilioConfig.AccountSid, _twilioConfig.AuthToken);
        foreach (var phoneNumber in phoneNumbers)
        {
            try
            {
                var sanitized = PhoneNumberUtils.SanitizePhoneNumber(phoneNumber, _configuration.GetSmsCountryCode());
                var sendResult = await MessageResource.CreateAsync("+" + sanitized, body: body, messagingServiceSid: serviceId);
                if (sendResult.ErrorCode != null)
                {
                    failed += phoneNumber;
                    errorCount++;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Twilio error");
            }

        }


        if (errorCount == phoneNumbers.Count)
        {
            return new SendResponse() { Code = "500", Desc = "All Failed" };
        }

        if (errorCount > 0 && errorCount < phoneNumbers.Count)
        {
            return new SendResponse() { Code = "501", Desc = "Partial Fail - " + failed };
        }

        return new SendResponse() { Code = "200", Desc = "Success" };
    }
}
