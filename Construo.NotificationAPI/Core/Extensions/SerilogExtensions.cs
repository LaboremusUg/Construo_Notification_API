using Construo.NotificationAPI.Core.Helpers;
using Serilog;
using Serilog.Core;

namespace Construo.NotificationAPI.Core.Extensions;

public static class SerilogExtensions
{
    public static void AddLoggingServices(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((ctx, services, lc) =>
        {
            var seqSettings = ctx.Configuration.GetSeqSettings();
            if (seqSettings.Enabled)
            {
                lc.WriteTo.Seq(
                    seqSettings.BaseUrl,
                    apiKey: seqSettings.ApiKey,
                    controlLevelSwitch: new LoggingLevelSwitch()
                    );
            }

        });
    }
}

