using DotNetExtensions.Common;
using DotNetExtensions.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class IpWhiteListMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string path;
        private readonly string method;
        private readonly IEnumerable<string> whiteList;

        public IpWhiteListMiddleware(
            RequestDelegate next,
            IEnumerable<string> whiteList, 
            string path, 
            string method)
        {
            this.next = next;
            this.whiteList = whiteList.ReturnOrThrowIfNull(
                "IP white list cannot be null.");
            this.path = path;
            this.method = method;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var methodMatches = this.method.IsNullOrWhiteSpace() ||
                this.method == context.Request.Method;

            if (!methodMatches)
            {
                await this.next.Invoke(context);

                return;
            }

            var pathMatches = this.path.IsNullOrWhiteSpace() ||
                context.Request.Path.Equals(this.path, StringComparison.OrdinalIgnoreCase);

            if (!pathMatches)
            {
                await this.next.Invoke(context);

                return;
            }

            var remoteIp = context.Connection.RemoteIpAddress;
            var remoteIpBytes = remoteIp.GetAddressBytes();
            var accessDenied = true;

            foreach (var whitelistedIp in this.whiteList)
            {
                var isValidIp = IPAddress.TryParse(
                    whitelistedIp, out IPAddress safeIpAddress);

                if (!isValidIp)
                {
                    continue;
                }

                if (safeIpAddress.GetAddressBytes().SequenceEqual(remoteIpBytes))
                {
                    accessDenied = false;

                    break;
                }
            }

            if (accessDenied)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                await context.Response.WriteAsync("IP address is not white-listed.");

                return;
            }

            await this.next.Invoke(context);
        }
    }
}
