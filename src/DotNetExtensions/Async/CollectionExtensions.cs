using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetExtensions.Async
{
    public static class CollectionExtensions
    {
        public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks).GetAwaiter();
        }

        public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks).GetAwaiter();
        }
    }
}
