using System.Net;

namespace Construo.NotificationAPI.Core.Helpers;

public class CustomResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
    public List<string> Errors { get; set; }
}

public class ErrorResponse
{
    public HttpStatusCode Code { get; set; }
    public string Message { get; set; }
}