using DotNetExtensions.AspNetCore.Common;
using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class ValidateBearerTokenMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string path;
        private readonly string token;
        private readonly string method;

        public ValidateBearerTokenMiddleware(
            RequestDelegate next, 
            string method, 
            string token, 
            string path = default)
        {
            this.next = next;
            this.method = method;
            this.token = token;
            this.path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != this.method)
            {
                await this.next.Invoke(context);

                return;
            }

            var pathMatches = this.path.IsNullOrWhiteSpace() ||
                context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase);

            if (!pathMatches)
            {
                await this.next.Invoke(context);

                return;
            }

            if (!context.Request.Headers.HasAuthorizationHeader())
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                await context.Response.WriteAsync("Authorization header is missing.");

                return;
            }

            var bearerToken = context.Request.Headers.ExtractBearerToken();

            if (this.token != bearerToken)
            {
                var message = "Invalid authorization token.";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                await context.Response.WriteAsync(message);

                return;
            }

            await this.next.Invoke(context);
        }
    }
}
