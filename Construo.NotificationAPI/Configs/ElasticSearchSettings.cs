using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Collections.Specialized;

namespace Construo.NotificationAPI.Configs;

public class ElasticSearchSettings
{
    public string Service { get; set; }
    public string Server { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
    public bool HealthChecks { get; set; }
    public ElasticsearchSinkOptions GetOptions()
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{UserName}:{Password}");
        var elasticToken = Convert.ToBase64String(plainTextBytes);
        return new ElasticsearchSinkOptions(new Uri(Server))
        {
            MinimumLogEventLevel = LogEventLevel.Information,
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            BufferBaseFilename = @"Logs\buffer",
            EmitEventFailure = EmitEventFailureHandling.ThrowException,
            ModifyConnectionSettings = c =>
                c.GlobalHeaders(new NameValueCollection { { "Authorization", $"Basic {elasticToken}" } })
        };
    }
}
