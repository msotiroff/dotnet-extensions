using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.Middlewares
{
    public class EnableRewindMiddleware
    {
        private readonly RequestDelegate next;

        public EnableRewindMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stream = context.Request.Body;

            if (stream == Stream.Null)
            {
                await this.next(context);

                return;
            }

            try
            {
                using (var buffer = new MemoryStream())
                {
                    await stream.CopyToAsync(buffer);

                    buffer.Position = 0L;

                    context.Request.Body = buffer;

                    await this.next(context);
                }
            }
            finally
            {
                context.Request.Body = stream;
            }
        }
    }
}
