#if NETSTANDARD2_0
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SpamBlock
{
    public class SpamBlockMiddleware
    {

        private readonly RequestDelegate _next;

        public SpamBlockMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.

            await _next.Invoke(context);

            // Clean up.
        }
    }

    public static class SpamBlockMiddlewareExtensions
    {
        public static IApplicationBuilder UseSpamBlockMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SpamBlockMiddleware>();
        }
    }
}
#endif
