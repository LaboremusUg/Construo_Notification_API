using Construo.NotificationAPI.Data;
using Construo.NotificationAPI.Models;

namespace Construo.NotificationAPI.Repository;

public class MessageQueueRepository : GenericRepository<MessageQueueItem>, IMessageQueueRepository
{
    public MessageQueueRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
