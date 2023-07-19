using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class UseAdminApiKeyAttribute : Attribute, IAsyncActionFilter 
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("AdminAPIKey");

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
 