using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class LogRequestorIpAddressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Action<string> logAction;

        public LogRequestorIpAddressMiddleware(
            RequestDelegate next,
            Action<string> logAction)
        {
            this.next = next;
            this.logAction = logAction;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var ip = context.Connection.RemoteIpAddress;

            this.logAction($"Incoming {method} request from remote IP address '{ip}'");

            await this.next(context);
        }
    }
}
