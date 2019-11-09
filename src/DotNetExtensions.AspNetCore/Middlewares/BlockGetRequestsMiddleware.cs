using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class BlockGetRequestsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string path;

        public BlockGetRequestsMiddleware(RequestDelegate next, string path)
        {
            this.next = next;
            this.path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isGetRequest = context.Request.Method == HttpMethods.Get;
            var pathMatches = this.path.IsNullOrWhiteSpace() ||
                context.Request.Path.Equals(this.path, StringComparison.OrdinalIgnoreCase);
            var shouldTerminate = isGetRequest && pathMatches;

            if (shouldTerminate)
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;

                await context.Response.WriteAsync("GET requests not allowed.");

                return;
            }

            await this.next.Invoke(context);
        }
    }
}
