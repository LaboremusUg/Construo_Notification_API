using Construo.NotificationAPI.Data;
using Construo.NotificationAPI.Models.Sms;
using Microsoft.EntityFrameworkCore;

namespace Construo.NotificationAPI.Repository;

public class SmsLogRepository : ISmsLogRepository
{
    private readonly ApplicationDbContext _applicationDb;

    public SmsLogRepository(ApplicationDbContext applicationDb)
    {
        _applicationDb = applicationDb;
    }

    public async Task<List<SmsLog>> GetAllAsync()
    {
        return await _applicationDb.SmsLogs.ToListAsync();
    }

    public void Add(SmsLog smsLog)
    {
        _applicationDb.Add(smsLog);
    }
    public async Task<SmsLog> CreateAsync(SmsLog smsLog)
    {
        await _applicationDb.AddAsync(smsLog);
        await _applicationDb.SaveChangesAsync();
        return smsLog;
    }
    public async Task CreateManyAsync(List<SmsLog> smsLogs)
    {
        //var smsLogsList = smsLogs.ToList();
        await _applicationDb.AddRangeAsync(smsLogs);
        var affectedRows = await _applicationDb.SaveChangesAsync();
    }
    public void Update(SmsLog smsLog)
    {
        _applicationDb.Entry(smsLog).State = EntityState.Modified;
    }

    public async Task<bool> SaveChangesAsync()
    {
        var affectedRows = await _applicationDb.SaveChangesAsync();
        return affectedRows > 0;
    }
    public bool SaveChanges()
    {
        var affectedRows = _applicationDb.SaveChanges();
        return affectedRows > 0;
    }
}
