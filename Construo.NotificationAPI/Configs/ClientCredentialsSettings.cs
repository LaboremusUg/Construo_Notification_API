namespace Construo.NotificationAPI.Configs;

public class ClientCredentialsSettings
{
    public const string SectionName = "ClientCredentials";

    public string AuthDefinition { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string HeaderName { get; init; } = default!;

    public string TokenUrl { get; init; } = default!;

    public string ClientDefinition { get; init; } = default!;

    public List<Scope> Scopes { get; init; } = default!;

}

public class Scope
{
    public string Name { get; init; } = default!;

    public string Description { get; init; } = default!;

}
