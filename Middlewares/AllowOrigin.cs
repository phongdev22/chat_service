using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace YourNamespace.Middlewares
{
    public class AllowOrigin
    {
        private readonly RequestDelegate _next;

        public AllowOrigin(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // context.Response.Headers.Add("Access-Control-Allow-Origin", "http://127.0.0.1:5500");

            context.Response.Headers.Remove("Access-Control-Allow-Origin");
            context.Response.Headers.Remove("Access-Control-Allow-Methods");
            context.Response.Headers.Remove("Access-Control-Allow-Headers");
            context.Response.Headers.Remove("Access-Control-Allow-Credentials");

            await _next(context);
        }
    }
}
