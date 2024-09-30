using Construo.NotificationAPI.Models.Sms;

namespace Construo.NotificationAPI.Repository;

public interface ISmsLogRepository
{
    /// <summary>
    /// Marks the model for addition to the Db. Doesn't persist the change to the Db. Call the SaveChangesAsync method to do so
    /// </summary>
    /// <param name="smsLog"></param>
    void Add(SmsLog smsLog);

    /// <summary>
    /// Saves a SINGLE new SmsLog to the database
    /// </summary>
    /// <param name="smsLog"></param>
    /// <returns></returns>
    Task<SmsLog> CreateAsync(SmsLog smsLog);
    /// <summary>
    /// Saves a batch of SmsLogs to the database
    /// </summary>
    /// <param name="smsLogs"></param>
    /// <returns></returns>
    Task CreateManyAsync(List<SmsLog> smsLogs);

    /// <summary>
    /// Marks the model as modified for updating in the DB. Doesn't persist the change to the Db. Call the SaveChangesAsync method to do so
    /// </summary>
    /// <param name="smsLog"></param>
    void Update(SmsLog smsLog);

    /// <summary>
    /// Persists all the changes (additions, updates etc) to the Db at once asynchronously
    /// </summary>
    /// <returns></returns>
    Task<bool> SaveChangesAsync();
    /// <summary>
    /// Persists all the changes (additions, updates etc) to the Db at once synchronously
    /// </summary>
    /// <returns></returns>
    bool SaveChanges();
}
