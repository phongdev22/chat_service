using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace YourNamespace.Middlewares
{
    public class CheckAccessToken
    {
        private readonly RequestDelegate _next;

        public CheckAccessToken(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? accessToken = context.Request.Headers["access_token"];

            if (string.IsNullOrEmpty(accessToken) && context.Request.Path != "/Home" && context.Request.Path != "/Account/login")
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Access token must be require!"
                };

                string jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            await _next(context);
        }
    }

    public static class CheckAccessTokenExtensions
    {
        public static IApplicationBuilder UseCheckAccessToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckAccessToken>();
        }
    }
}
