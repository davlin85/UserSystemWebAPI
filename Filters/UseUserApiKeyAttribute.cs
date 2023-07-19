using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Filters
{
    public class UseUserApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("UserAPIKey");

            if (!context.HttpContext.Request.Query.TryGetValue("code", out var code))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!apiKey.Equals(code))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
