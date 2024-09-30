using Construo.NotificationAPI.Core.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog.Context;
using System.Net;

namespace Construo.NotificationAPI.Core.Extensions;

public class ErrorHandling
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly ILogger<ErrorHandling> _logger;

    public ErrorHandling(RequestDelegate next, IConfiguration config, ILogger<ErrorHandling> logger)
    {
        _next = next;
        _config = config;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context /* other scoped dependencies */)
    {
        var correlationId = context.Request.Headers["X-Correlation-Id"].ToString();
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var showDetailedErrors = _config.GetValue<bool>("ShowDetailedErrors");
        _logger.LogError(exception, "global.error");
        var code = HttpStatusCode.InternalServerError;

        var message = "Unknown error, please contact administrator";
        if (showDetailedErrors)
        {
            message = exception.StackTrace;
        }
        else
        {
            switch (exception)
            {
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case ClientFriendlyException _:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case DbUpdateException dbe:
                    code = HttpStatusCode.BadRequest;
                    if (dbe.Message.Contains("duplicate key") ||
                        (dbe.InnerException?.Message?.Contains("duplicate key") ?? false))
                        message = "Duplicate entry";
                    break;
                case SqlException ex:
                    code = HttpStatusCode.BadRequest;
                    if (ex.Message.Contains("duplicate key"))
                        message = "Duplicate entry";
                    break;
            }
        }

        var result = JsonConvert.SerializeObject(new { code, message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(result);

    }
}
