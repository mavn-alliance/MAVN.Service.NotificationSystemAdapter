using System.Text;
using MAVN.Service.NotificationSystemAdapter.Strings;
using Microsoft.AspNetCore.Http;

namespace MAVN.Service.NotificationSystemAdapter.Infrastructure
{
    public class RequestContext : IRequestContext
    {
        private readonly HttpContext _httpContext;

        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string ApiKey => GetApiKey();

        public void ResponseUnauthorized(string customMessage = null)
        {
            _httpContext.Response?.OnStarting(async () =>
            {
                _httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                var message = customMessage ?? Phrases.NotAuthenticated;
                await _httpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(message), 0, message.Length);
            });
        }

        private string GetApiKey()
        {
            var header = _httpContext.Request.Headers["api-key"].ToString();

            if (string.IsNullOrEmpty(header))
                return null;

            return header;
        }
    }
}
