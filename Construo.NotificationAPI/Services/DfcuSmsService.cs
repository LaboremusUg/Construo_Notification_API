using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Services;

public class DfcuSmsService : ISmsServiceProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmsService> _logger;
    private const string Msisdn = "MSISDN";
    private const string Message = "MESSAGE";
    private const string Username = "USERNAME";
    private const string Password = "PASSWORD";
    public DfcuSmsService(IConfiguration configuration, ILogger<SmsService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<SendResponse> SendAsync(List<string> phoneNumbers, string message, string username, string password, string senderId)
    {
        _logger.LogInformation("sending via dfcu service");
        using (var client = new HttpClient())
        {
            var sendResponse = new SendResponse();
            var escapedMessage = Uri.EscapeDataString(message);
            var url = $"{_configuration.GetDfcuUrl()}";

            foreach (var phoneNumber in phoneNumbers)
            {
                var dict = new Dictionary<string, string>
                    {
                        {Message, escapedMessage},
                        {Username, username},
                        {Password, password},
                        {Msisdn, phoneNumber},
                        {"api_id", "0" },
                        {"from", senderId }
                    };
                var getUrl = $"{url}?api_id=0&from={senderId}&{Message}={escapedMessage}&{Username}={username}&{Password}={password}&{Msisdn}={phoneNumber}";
                _logger.LogDebug(getUrl);
                var response = await client.GetAsync(getUrl);
                _logger.LogInformation($"dfcu.response Code:{response.StatusCode}");
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"result : {result}");
                response.EnsureSuccessStatusCode();
                sendResponse.Code = result.ToLower().Contains("success") ? "200" : "500";
                sendResponse.Desc = result;
            }
            return sendResponse;
        }
    }
}
