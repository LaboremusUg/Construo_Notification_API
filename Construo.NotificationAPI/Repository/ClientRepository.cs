using Construo.NotificationAPI.Data;
using Construo.NotificationAPI.Models.Sms;
using Microsoft.EntityFrameworkCore;

namespace Construo.NotificationAPI.Repository;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public new async Task<Client> GetById(Guid id)
    {
        return await _dbContext.Clients.Include(c => c.ServiceProvider).SingleAsync(c => c.Id == id);
    }
}
