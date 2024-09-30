using Construo.NotificationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Construo.NotificationAPI.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;
    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<TEntity> GetById(Guid id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> CreateAsync(TEntity client)
    {
        _dbContext.Set<TEntity>().Add(client);
        try
        {
            await _dbContext.SaveChangesAsync();
            return client;
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }
}
