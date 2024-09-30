using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Services;

public interface ISmsService
{
    Task<SendResponse> SendAsync(Sms sms);
}
