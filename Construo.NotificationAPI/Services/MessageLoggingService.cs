using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Repository;

namespace Construo.NotificationAPI.Services;

public class MessageLoggingService : IMessageLoggingService
{
    private readonly IMessageQueueRepository _messageQueueRepository;

    public MessageLoggingService(IMessageQueueRepository messageQueueRepository)
    {
        _messageQueueRepository = messageQueueRepository;
    }

    public async Task<MessageQueueItem> CreateAsync(MessageQueueItem messageQueueItem)
    {
        return await _messageQueueRepository.CreateAsync(messageQueueItem);
    }

    public async Task<MessageQueueItem> GetByIdAsync(Guid id)
    {
        return await _messageQueueRepository.GetById(id);
    }

    public async Task UpdateAsync(MessageQueueItem message)
    {
        await _messageQueueRepository.UpdateAsync(message);
    }
}
