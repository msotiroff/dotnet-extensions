using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DotNetExtensions.Async
{
    public static class TimeSpanExtensions
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero)
            {
                return Task.Delay(TimeSpan.Zero).GetAwaiter();
            }

            return Task.Delay(timeSpan).GetAwaiter();
        }
    }
}
