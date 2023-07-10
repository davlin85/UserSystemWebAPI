using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class UseAdminApiKeyFilterAttribute : Attribute, IAsyncActionFilter 
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException(); 
        }
    }
}
