using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Musicality.Telegram;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class TelegramSecretTokenAuthorize : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<TelegramOptions>>();

        if (string.IsNullOrEmpty(options.Value.SecretToken))
        {
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var value))
        {
            context.Result = new UnauthorizedObjectResult(new { Error = "Security token is missing" });
            return;
        }

        if (value != options.Value.SecretToken)
        {
            context.Result = new UnauthorizedObjectResult(new { Error = "Security token is invalid" });
            return;
        }
    }
}
