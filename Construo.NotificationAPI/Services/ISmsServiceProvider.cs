using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Services;

public interface ISmsServiceProvider
{
    /// <summary>
    /// Performs implementation for a given service provider
    /// </summary>
    /// <param name="phoneNumbers"></param>
    /// <param name="message"></param>
    /// <param name="senderId"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<SendResponse> SendAsync(List<string> phoneNumbers, string message, string username, string password, string senderId = null);
}
