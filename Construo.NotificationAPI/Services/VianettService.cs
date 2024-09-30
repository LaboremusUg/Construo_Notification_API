using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.Models.Sms;
using Construo.NotificationAPI.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Web;

namespace Construo.NotificationAPI.Services;

public class VianettService : ISmsServiceProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmsService> _smsServiceLogger;

    public VianettService(IConfiguration configuration, ILogger<SmsService> smsServiceLogger)
    {
        _configuration = configuration;
        _smsServiceLogger = smsServiceLogger;
    }

    public async Task<SendResponse> SendAsync(List<string> phoneNumbers, string message, string username, string password, string senderId = null)
    {
        var sendResponse = new SendResponse
        {
            Code = "500",
            Desc = "Fail"
        };
        var url = $"{_configuration.VianettUrl()}";
        _smsServiceLogger.LogInformation($"Sending Vianett SMS with URL:{url}");
        var campaignId = _configuration["Providers:Vianett:CampaignId"];
        var model = new VianettSms
        {
            Tel = string.Join(";", phoneNumbers),
            Msg = HttpUtility.UrlEncode(message, Encoding.UTF8),
            Username = username,
            Password = password,
        };
        model.CampaignId = !string.IsNullOrEmpty(campaignId) ? campaignId : null;
        model.SenderAddress = string.IsNullOrEmpty(senderId) ? _configuration["Providers:Vianett:GlobalSender"] : senderId;
        using (var client = new HttpClient())
        {
            var jsonData = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                ContractResolver = new LowerCaseContractResolver()
            });
            var keyValueData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            var urlParams = string.Join("&", keyValueData.Select(x => $"{x.Key}={x.Value}"));
            url += $"?{urlParams}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var sendResult = await response.Content.ReadAsStringAsync();
                sendResponse = ParseServerResult(sendResult);
            }
            else
            {
                _smsServiceLogger.LogError($"Failed to send SMS with error:{response.StatusCode}|{response.ReasonPhrase}");
            }
        }

        return sendResponse;
    }

    private SendResponse ParseServerResult(string serverResult)
    {
        SendResponse sendResponse = new SendResponse();
        var result = serverResult.Split("|");
        sendResponse.Code = result[0];
        sendResponse.Desc = result[1].Equals("OK") ? "Success" : result[1];
        return sendResponse;
    }
}

public class LowerCaseContractResolver : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToLower();
    }
}