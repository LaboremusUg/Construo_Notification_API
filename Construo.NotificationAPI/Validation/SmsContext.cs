using FluentValidation;
using Construo.NotificationAPI.ViewModels;

namespace Construo.NotificationAPI.Validation;

public class SmsContext : AbstractValidator<Sms>
{
    public SmsContext()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.Recipients).Must(t => t.Count > 0);
        RuleFor(x => x.ClientId).NotEmpty();
    }
}
