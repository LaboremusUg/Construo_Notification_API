namespace Construo.NotificationAPI.Core.Extensions;

public static class StringExtensions
{
    public static string Or(this string str, string def)
    {
        return string.IsNullOrWhiteSpace(str) ? def : str;
    }
}
