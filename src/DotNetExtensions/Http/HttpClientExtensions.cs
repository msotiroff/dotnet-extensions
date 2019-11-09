using System;
using System.Net.Http;
using System.Threading;

namespace DotNetExtensions.Http
{
    public static class HttpClientExtensions
    {
        public static void TrySetTimeout(this HttpClient client, TimeSpan timeout)
        {
            try
            {
                client.Timeout = timeout;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Sets the request timeout to infinite.
        /// IMPORTANT: Use only if you pass a cancellation token to client's calls.
        /// </summary>
        public static void TrySetInfiniteTimeout(this HttpClient client)
        {
            client.TrySetTimeout(Timeout.InfiniteTimeSpan);
        }
    }
}
