using Construo.NotificationAPI.Models;

namespace Construo.NotificationAPI.Services;

public interface IEmailService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email">Email Object</param>
    /// <param name="jobId"></param>
    /// <see cref="EmailDetails"/>
    /// <returns></returns>
    Task<MessageServiceResult> SendMailAsync(EmailDetails email, Guid jobId);
}
