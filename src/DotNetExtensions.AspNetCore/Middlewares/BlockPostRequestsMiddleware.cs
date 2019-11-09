using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class BlockPostRequestsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string path;

        public BlockPostRequestsMiddleware(RequestDelegate next, string path)
        {
            this.next = next;
            this.path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isPostRequest = context.Request.Method == HttpMethods.Post;
            var pathMatches = this.path.IsNullOrWhiteSpace() ||
                context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase);
            var shouldTerminate = isPostRequest && pathMatches;

            if (shouldTerminate)
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;

                await context.Response.WriteAsync("POST requests not allowed.");

                return;
            }

            await this.next.Invoke(context);
        }
    }
}
