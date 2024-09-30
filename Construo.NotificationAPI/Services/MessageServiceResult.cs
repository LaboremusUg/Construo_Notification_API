namespace Construo.NotificationAPI.Services;

/// <summary>
/// Represents the result of a mail send operation.
/// </summary>
public class MessageServiceResult
{
    /// <inheritdoc />
    /// <summary>
    /// Failure constructor that takes error messages.
    /// </summary>
    /// <param name="errors"></param>
    public MessageServiceResult(params string[] errors)
        : this((IEnumerable<string>)errors)
    {
    }

    /// <summary>
    /// Failure constructor that takes error messages
    /// </summary>
    /// <param name="errors"></param>
    public MessageServiceResult(IEnumerable<string> errors)
    {
        if (errors == null)
        {
            errors = new[] { "Error occured in message service." };
        }
        Succeeded = false;
        Errors = errors;
    }

    /// <summary>
    /// Constructor that takes whether the result is successful
    /// </summary>
    /// <param name="success"></param>
    protected MessageServiceResult(bool success)
    {
        Succeeded = success;
        Errors = new string[0];
    }

    /// <summary>
    /// True if the operation was successful
    /// </summary>
    public bool Succeeded { get; private set; }

    /// <summary>
    /// List of errors
    /// </summary>
    public IEnumerable<string> Errors { get; private set; }

    /// <summary>
    /// Static success result
    /// </summary>
    /// <returns></returns>
    public static MessageServiceResult Success { get; } = new MessageServiceResult(true);

    /// <summary>
    /// Failed helper method
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static MessageServiceResult Failed(params string[] errors)
    {
        return new MessageServiceResult(errors);
    }
}