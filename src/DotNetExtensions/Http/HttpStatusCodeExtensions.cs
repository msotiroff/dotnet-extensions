using System.Net;

namespace DotNetExtensions.Http
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessfull(this HttpStatusCode statusCode)
        {
            return (int)statusCode >= 200 && (int)statusCode < 300;
        }
    }
}
