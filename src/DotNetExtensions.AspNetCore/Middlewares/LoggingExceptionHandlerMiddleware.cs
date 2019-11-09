using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class LoggingExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoggingExceptionHandlerMiddleware> logger;
        private readonly IHostingEnvironment environment;

        public LoggingExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<LoggingExceptionHandlerMiddleware> logger, 
            IHostingEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    $"Exception thrown.{Environment.NewLine}" +
                    $"Message: {ex.Message}{Environment.NewLine}" +
                    $"Stack trace: {ex.StackTrace}");

                if (this.environment.IsDevelopment())
                {
                    throw;
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
