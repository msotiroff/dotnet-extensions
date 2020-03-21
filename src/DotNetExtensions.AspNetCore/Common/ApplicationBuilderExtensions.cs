using DotNetExtensions.AspNetCore.Middlewares;
using DotNetExtensions.Common;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace DotNetExtensions.AspNetCore.Common
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder BlockGetRequests(
            this IApplicationBuilder builder, string specificPath = default)
        {
            specificPath = specificPath.IsNull() ? string.Empty : specificPath;

            builder.UseMiddleware<BlockGetRequestsMiddleware>(specificPath);

            return builder;
        }

        public static IApplicationBuilder BlockPostRequests(
            this IApplicationBuilder builder, string specificPath = default)
        {
            specificPath = specificPath.IsNull() ? string.Empty : specificPath;

            builder.UseMiddleware<BlockPostRequestsMiddleware>(specificPath);

            return builder;
        }

        public static IApplicationBuilder ValidateBearerToken(
            this IApplicationBuilder builder, 
            string httpMethod, 
            string token, 
            string specificPath = default)
        {
            specificPath = specificPath.IsNull() ? string.Empty : specificPath;

            builder.UseMiddleware<ValidateBearerTokenMiddleware>(
                httpMethod, token, specificPath);

            return builder;
        }

        public static IApplicationBuilder EnableRequestBodyRewind(
            this IApplicationBuilder builder)
        {
            builder.UseMiddleware<EnableRewindMiddleware>();

            return builder;
        }

        public static IApplicationBuilder UseIpWhiteList(
            this IApplicationBuilder builder, 
            IEnumerable<string> whiteList, 
            string specificPath = default, 
            string httpMethod = default)
        {
            specificPath = specificPath.IsNull() ? string.Empty : specificPath;

            builder.UseMiddleware<IpWhiteListMiddleware>(
                whiteList, specificPath, httpMethod);

            return builder;
        }

        public static IApplicationBuilder UseLoggingExceptionHandler(
            this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LoggingExceptionHandlerMiddleware>();

            return builder;
        }

        public static IApplicationBuilder UseAnonymousBrowser(this IApplicationBuilder app)
        {
            app.UseMiddleware<AnonymousBrowserMiddleware>();

            return app;
        }
    }
}
