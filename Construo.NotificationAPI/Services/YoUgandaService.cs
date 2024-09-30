using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Services;

public class YoUgandaService : ISmsServiceProvider
{
    private readonly string _submitUrl;
    public YoUgandaService(IConfiguration configuration)
    {
        _submitUrl = configuration.YoUgandaUrl();
    }


    /// <inheritdoc />
    /// <summary>
    /// Yo implementation for sending an SMS
    /// </summary>
    /// <param name="phoneNumbers">A list of phone numbers</param>
    /// <param name="message">The message to send</param>
    /// <param name="senderId">Customized sender id</param>
    /// <param name="username">Yo Account number</param>
    /// <param name="password">Yo account password</param>
    /// <returns></returns>
    public async Task<SendResponse> SendAsync(List<string> phoneNumbers, string message, string username, string password, string senderId = null)
    {
        var sendResponse = new SendResponse();
        using (var client = new HttpClient())
        {
            var destinations = string.Join(",", phoneNumbers.Select(x => PhoneNumberUtils.SanitizePhoneNumber(x) != "-1"));
            var requestUri = $"{_submitUrl}?ybsacctno={username}&password={password}&origin={senderId}&sms_content={Uri.EscapeDataString(message)}&destinations={destinations}&nostore=1";
            var response = await client.GetStringAsync(requestUri);
            var yoResponse = ProcessYoResponse(Uri.UnescapeDataString(response));
            if (yoResponse["ybs_autocreate_status"].ToUpper() == "OK")
            {
                sendResponse.Code = "200";
                sendResponse.Desc = "Success";
            }
            else
            {
                sendResponse.Code = "500";
                sendResponse.Desc = yoResponse["ybs_autocreate_message"];
            }
        }
        return sendResponse;
    }
    /// <summary>
    /// Parses the response from the server into a dictionary
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public Dictionary<string, string> ProcessYoResponse(string response)
    {
        var respArray = response.Split(Char.Parse("&"));
        var output = new Dictionary<string, string>();
        foreach (var item in respArray)
        {
            var splitAtEqualSign = item.Split(Char.Parse("="));
            output.Add(splitAtEqualSign[0], splitAtEqualSign[1]);
        }
        return output;
    }
}
