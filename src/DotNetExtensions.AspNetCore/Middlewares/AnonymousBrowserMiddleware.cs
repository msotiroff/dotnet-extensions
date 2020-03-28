using DotNetExtensions.AspNetCore.Common;
using DotNetExtensions.AspNetCore.Internal;
using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;
using System;
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

            if (browserCookie.IsNullOrWhiteSpace())
            {
                context.SetNewBrowserId(expiration);
            }

            await this.next.Invoke(context);
        }
    }
}
