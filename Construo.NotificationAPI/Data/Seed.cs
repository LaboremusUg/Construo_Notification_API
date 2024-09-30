using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Models.Sms;
using Microsoft.EntityFrameworkCore;
namespace Construo.NotificationAPI.Data;

public static class Seed
{
    public static void InitDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        var serviceProviderDbList = context.ServiceProviders.ToList();

        foreach (var value in Enum.GetValues(typeof(SmsServiceProviderType)))
        {
            var name = Enum.GetName(typeof(SmsServiceProviderType), value);
            var serviceProvider = new SmsServiceProvider()
            {
                Id = (int)value,
                Name = name
            };

            var existingProvider = serviceProviderDbList.FirstOrDefault(x => x.Id == serviceProvider.Id);
            if (existingProvider != null)
            {
                if (existingProvider.Name != serviceProvider.Name)
                {
                    existingProvider.Name = serviceProvider.Name;
                    context.ServiceProviders.Update(existingProvider);
                }
            }
            else
            {
                context.ServiceProviders.Add(serviceProvider);
            }
        }

        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"ServiceProviders\"', 'Id'), MAX(\"Id\")) FROM \"ServiceProviders\"");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

