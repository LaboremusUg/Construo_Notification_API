using Hangfire.Dashboard;

namespace Construo.NotificationAPI.Core.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        //var httpContext = context.GetHttpContext();

        // Allow all authenticated users to see the Dashboard
        // return httpContext.User.Identity.IsAuthenticated;
        return true;
    }
}
