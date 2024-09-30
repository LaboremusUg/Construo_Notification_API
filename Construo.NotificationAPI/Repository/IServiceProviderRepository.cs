using Construo.NotificationAPI.Models.Sms;
using System.Linq.Expressions;

namespace Construo.NotificationAPI.Repository;

public interface IServiceProviderRepository : IGenericRepository<SmsServiceProvider>
{
    IEnumerable<SmsServiceProvider> Search(Expression<Func<SmsServiceProvider, bool>> predicate);
    Task<SmsServiceProvider> GetByIdAsync(int id);
}
