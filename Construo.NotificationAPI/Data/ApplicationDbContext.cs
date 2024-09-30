using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Models.Sms;
using Microsoft.EntityFrameworkCore;

namespace Construo.NotificationAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SmsLog> SmsLogs { get; set; }
    public DbSet<Models.Sms.SmsServiceProvider> ServiceProviders { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DbSet<MessageQueueItem> MessageQueueItems { get; set; }
}
