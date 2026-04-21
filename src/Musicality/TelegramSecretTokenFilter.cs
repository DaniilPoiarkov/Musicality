using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Musicality;

internal sealed class TelegramSecretTokenFilter(IConfiguration configuration) : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var secret = configuration["Telegram:SecretToken"];
        if (string.IsNullOrEmpty(secret))
        {
            return next();
        }

        if (!context.HttpContext.Request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var values)
            || values.Count != 1
            || values[0] != secret)
        {
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        return next();
    }
}
