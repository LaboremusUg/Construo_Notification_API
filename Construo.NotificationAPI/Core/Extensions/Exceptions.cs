namespace Construo.NotificationAPI.Core.Extensions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

[Serializable]
public class ApplicationValidationException : Exception
{
    public ApplicationValidationException(string message) : base(message) { }
}