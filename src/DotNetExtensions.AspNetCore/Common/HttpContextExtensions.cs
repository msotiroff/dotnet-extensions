using DotNetExtensions.AspNetCore.Internal;
using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;
using System;

namespace DotNetExtensions.AspNetCore.Common
{
    public static class HttpContextExtensions
    {
        public static string GetBrowserId(this HttpContext context)
        {
            return context.Request.Cookies[Constants.AnonymounsBrowserCookieName];
        }

        public static string GetOrAddBrowserId(this HttpContext context)
        {
            var browserId = context.Request.Cookies[Constants.AnonymounsBrowserCookieName];

            if (browserId.IsNotNullOrWhiteSpace())
            {
                return browserId;
            }

            browserId = new TripleGuid().ToString();

            context.SetNewBrowserId(TimeSpan.FromDays(366), browserId);

            return browserId;
        }

        public static void SetNewBrowserId(
            this HttpContext context, 
            TimeSpan expiration, 
            string browserId = default)
        {
            context.Response.Cookies.Append(
                Constants.AnonymounsBrowserCookieName,
                browserId ?? new TripleGuid().ToString(),
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.Add(expiration)
                });
        }

        public static void ClearBrowserId(this HttpContext context)
        {
            if (context.Request.Cookies[Constants.AnonymounsBrowserCookieName].IsNotNull())
            {
                context.Response.Cookies.Delete(Constants.AnonymounsBrowserCookieName);
            }
        }
    }
}
