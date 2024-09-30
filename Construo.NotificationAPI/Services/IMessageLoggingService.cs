using Construo.NotificationAPI.Models;

namespace Construo.NotificationAPI.Services;

public interface IMessageLoggingService
{
    Task<MessageQueueItem> CreateAsync(MessageQueueItem messageQueueItem);
    Task<MessageQueueItem> GetByIdAsync(Guid id);
    Task UpdateAsync(MessageQueueItem message);
}
