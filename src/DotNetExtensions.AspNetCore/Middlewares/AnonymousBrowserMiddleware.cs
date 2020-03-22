using DotNetExtensions.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class AnonymousBrowserMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TimeSpan expiration;

        public AnonymousBrowserMiddleware(
            RequestDelegate next,
            TimeSpan expiration)
        {
            this.next = next;
            this.expiration = expiration;
        }

        public async Task Invoke(HttpContext context)
        {
            var browserCookie = context.Request.Cookies[Constants.AnonymounsBrowserCookieName];
            if (string.IsNullOrWhiteSpace(browserCookie))
            {
                context.Response.Cookies
                    .Append(
                    Constants.AnonymounsBrowserCookieName,
                    this.GenerateUniqueId(),
                    new CookieOptions
                    {
                        Expires = DateTime.UtcNow.Add(this.expiration)
                    });
            }

            await this.next.Invoke(context);
        }

        private string GenerateUniqueId()
        {
            var guids = Enumerable.Range(1, 3)
                .Select(n => Guid.NewGuid().ToString("N"));

            return string.Join(string.Empty, guids);
        }
    }
}
