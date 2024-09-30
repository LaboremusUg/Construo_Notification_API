namespace Construo.NotificationAPI.Core.Extensions;

public static class ErrorHandlingExtensions
{
    public static IApplicationBuilder UseCustomErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandling>();
    }
}
