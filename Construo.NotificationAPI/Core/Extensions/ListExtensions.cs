namespace Construo.NotificationAPI.Core.Extensions;

public static class ListExtensions
{
    public static string CommaSeparated(this List<string> list)
    {
        if (list == null || !list.Any())
        {
            return string.Empty;
        }
        return string.Join(",", list);
    }
}
