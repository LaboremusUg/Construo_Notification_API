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

        // Apply any pending migrations
        context.Database.Migrate();

        // Get all current service providers from the database
        var serviceProviderDbList = context.ServiceProviders.ToList();

        // Iterate through the enum values and add or update as necessary
        foreach (var value in Enum.GetValues(typeof(SmsServiceProviderType)))
        {
            var name = Enum.GetName(typeof(SmsServiceProviderType), value);
            var serviceProvider = new SmsServiceProvider()
            {
                Id = (int)value,
                Name = name
            };

            // Check if a record with the same Id but different Name exists and update it
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
                // Insert new record if the Id does not exist
                context.ServiceProviders.Add(serviceProvider);
            }
        }

        // Commit changes in a transaction
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                // Save changes to the database
                context.SaveChanges();

                // Optionally, reset the sequence for the Id column if necessary
                context.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"ServiceProviders\"', 'Id'), MAX(\"Id\")) FROM \"ServiceProviders\"");

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception)
            {
                // Rollback the transaction if something goes wrong
                transaction.Rollback();
                throw;  // Re-throw the exception for further handling/logging
            }
        }
    }
}

