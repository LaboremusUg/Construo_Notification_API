namespace Construo.NotificationAPI.ViewModels;

public class SendResponse
{
    /// <summary>
    /// A simple description of the response. Could be:
    /// Success
    /// Failed
    /// Partial Fail
    /// </summary>
    public string Desc { get; set; }
    /// <summary>
    /// A number respresenting the response status
    /// 200: Success
    /// 500: Complete failure to send to any of the specified numbers
    /// 501: Partial failure to send to some of the numbers. The desc will have a list of the failed numbers 
    /// </summary>
    public string Code { get; set; }
}
