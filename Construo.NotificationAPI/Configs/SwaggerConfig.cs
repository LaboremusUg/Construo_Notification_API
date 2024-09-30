namespace Construo.NotificationAPI.Configs;

public class SwaggerConfig
{
    public const string SectionName = "Swagger";

    public bool Enabled { get; init; } = default!;

    public bool UseSwaggerUi { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Version { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string ContactEmail { get; init; } = default!;

    public string ContactName { get; init; } = default!;

    public string ContactUrl { get; init; } = default!;

    public License License { get; init; } = default!;
}


public class License
{
    public string Name { get; init; } = default!;

    public string Url { get; init; } = default!;

}