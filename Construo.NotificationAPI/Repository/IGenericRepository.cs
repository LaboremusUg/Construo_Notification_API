namespace Construo.NotificationAPI.Repository;

public interface IGenericRepository<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(Guid id);
    Task UpdateAsync(TEntity entity);
}
