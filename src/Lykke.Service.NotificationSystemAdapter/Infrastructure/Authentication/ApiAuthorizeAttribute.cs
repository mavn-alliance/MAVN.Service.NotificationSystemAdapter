using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.NotificationSystemAdapter.Infrastructure.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public static string ValidApiKey { get; set; }

        private IRequestContext _requestContext;
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _requestContext = context.HttpContext.RequestServices.GetService<IRequestContext>();

            if (string.IsNullOrEmpty(_requestContext.ApiKey) || _requestContext.ApiKey != ValidApiKey)
            {
                _requestContext.ResponseUnauthorized();
            }
            else
            {
                await next();
            }
        }
    }
}
