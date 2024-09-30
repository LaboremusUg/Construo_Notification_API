namespace Construo.NotificationAPI.Configs;

public static class ConfigExtensions
{
    public static EmailConfiguration GetEmailConfiguration(this IConfiguration configuration)
    {
        return configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    }

    public static ElasticSearchSettings GetElasticSearchSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("ElasticSearch").Get<ElasticSearchSettings>();
    }

    public static SwaggerConfig GetSwagger(this IConfiguration configuration)
    {
        return configuration.GetSection("Swagger").Get<SwaggerConfig>();

    }

    public static AuthServiceSettings GetAuthServiceSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Authentication").Get<AuthServiceSettings>();

    }

    public static ClientCredentialsSettings GetClientCredentialsSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("ClientCredentials").Get<ClientCredentialsSettings>();

    }
}
