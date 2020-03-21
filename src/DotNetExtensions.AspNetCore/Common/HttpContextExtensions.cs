using DotNetExtensions.AspNetCore.Internal;
using DotNetExtensions.Common;
using Microsoft.AspNetCore.Http;

namespace DotNetExtensions.AspNetCore.Common
{
    public static class HttpContextExtensions
    {
        public static string GetBrowserId(this HttpContext context)
        {
            return context.Request.Cookies[Constants.AnonymounsBrowserCookieName];
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
