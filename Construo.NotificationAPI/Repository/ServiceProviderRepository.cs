using Construo.NotificationAPI.Data;
using Construo.NotificationAPI.Models.Sms;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Construo.NotificationAPI.Repository;

public class ServiceProviderRepository : GenericRepository<SmsServiceProvider>, IServiceProviderRepository
{
    private ApplicationDbContext _dbContext;

    public ServiceProviderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<SmsServiceProvider> Search(Expression<Func<SmsServiceProvider, bool>> predicate)
    {
        return _dbContext.ServiceProviders.Include(m => m.Clients).Where(predicate).ToList();
    }
    public async Task<SmsServiceProvider> GetByIdAsync(int id)
    {
        return await _dbContext.ServiceProviders.SingleAsync(m => m.Id == id);
    }
}
