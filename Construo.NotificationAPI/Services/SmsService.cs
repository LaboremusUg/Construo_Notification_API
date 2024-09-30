using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Models.Sms;
using Construo.NotificationAPI.Repository;
using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Services;

public class SmsService : ISmsService
{
    private ISmsServiceProvider _smsServiceProvider;
    private readonly ISmsLogRepository _smsLogRepository;
    private readonly IClientRepository _clientRepo;
    private string _providerName;
    private Client _client;
    private IConfiguration _configuration;
    private readonly ILogger<SmsService> _smsServiceLogger;

    public SmsService(ISmsLogRepository smsLogRepository, IClientRepository clientRepo, IConfiguration configuration, ILogger<SmsService> smsServiceLogger)
    {
        _smsLogRepository = smsLogRepository;
        _clientRepo = clientRepo;
        _configuration = configuration;
        _smsServiceLogger = smsServiceLogger;
    }

    private Client GetClient(Guid clientId)
    {
        _client = Task.Run(async () => await _clientRepo.GetById(clientId)).Result;
        return _client;
    }

    public async Task<SendResponse> SendAsync(Sms submittedSms)
    {
        _smsServiceProvider = GetServiceProvider(submittedSms.ClientId);
        if (_smsServiceProvider == null)
            return new SendResponse { Code = "500", Desc = "Specified client was not found" };
        List<string> validList = new List<string>();
        List<string> invalidList = new List<string>();
        var recipients = submittedSms.Recipients;
        var body = submittedSms.Body;
        foreach (var phone in recipients)
        {
            if (PhoneNumberUtils.IsValidPhoneNumber(phone))
                validList.Add(PhoneNumberUtils.SanitizePhoneNumber(phone));
            else
            {
                invalidList.Add(phone);
            }
        }

        if (invalidList.Count == recipients.Count)
        {
            _smsServiceLogger.LogInformation("All numbers are invalid");
            return new SendResponse { Code = "500", Desc = "Failed" };
        }

        if (validList.Count <= 0)
        {
            _smsServiceLogger.LogInformation("There are no valid numbers");
            return new SendResponse { Code = "500", Desc = "Failed" };
        }



        var smsLogs = new List<SmsLog>();
        foreach (var phone in validList)
        {
            _smsLogRepository.Add(new SmsLog
            {
                Message = body,
                SendDate = DateTime.UtcNow,
                PhoneNumber = phone,
                ServiceProvider = _providerName,
                ClientId = submittedSms.ClientId.ToString(),
            });
        }

        var saved = await _smsLogRepository.SaveChangesAsync();
        var sendResult = await _smsServiceProvider.SendAsync(validList, body, _client.ServiceProviderUsername,
            _client.ServiceProviderPassword, _client.SenderId);
        //If there were any invalid numbers then send the partial fail
        return invalidList.Count > 0
            ? new SendResponse() { Code = "501", Desc = "Partial Fail - " + string.Join(',', invalidList) }
            : sendResult;

    }

    /// <summary>
    /// Gets the client's set service provider
    /// </summary>
    /// <returns></returns>
    public ISmsServiceProvider GetServiceProvider(Guid clientId)
    {
        var client = GetClient(clientId);
        if (client == null)
            return null;
        var providerId = client.ServiceProviderId;
        var provider = Enum.Parse<SmsServiceProviderType>(providerId.ToString());
        _providerName = provider.ToString();
        return provider switch
        {
            SmsServiceProviderType.Twilio => new TwilioService(_configuration, _smsServiceLogger),
            SmsServiceProviderType.YoUganda => new YoUgandaService(_configuration),
            SmsServiceProviderType.ViaNett => new VianettService(_configuration, _smsServiceLogger),
            SmsServiceProviderType.Dfcu => new DfcuSmsService(_configuration, _smsServiceLogger),
            _ => throw new NotImplementedException(),
        };
    }
}
