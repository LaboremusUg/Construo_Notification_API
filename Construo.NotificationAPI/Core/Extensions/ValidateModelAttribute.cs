using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Construo.NotificationAPI.Core.Extensions;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsList = context.ModelState.Select(it =>
            {
                return (Field: it.Key, Errors: it.Value.Errors.Select(er => er.ErrorMessage).ToList());
            }).ToList();

            var errorDict = new Dictionary<string, List<string>>();
            foreach (var (field, errors) in errorsList)
            {
                errorDict[LowerInvariant(field)] = errors;
            }

            var error = new
            {
                Message = "The request is invalid",
                Errors = errorDict
            };
            context.Result = new BadRequestObjectResult(error);
        }
    }

    private string LowerInvariant(string word)
    {
        return char.ToLowerInvariant(word[0]) + word.Substring(1);
    }
}
