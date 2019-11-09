using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetExtensions.Common
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> GetAllMessages(this Exception ex)
        {
            if (ex.IsNull())
            {
                yield break;
            }

            yield return ex.Message;

            var innerExceptions = Enumerable.Empty<Exception>();

            if (ex is AggregateException aggregateExption && 
                aggregateExption.InnerExceptions.Any())
            {
                innerExceptions = aggregateExption.InnerExceptions;
            }
            else if (ex.InnerException.IsNotNull())
            {
                innerExceptions = new Exception[] { ex.InnerException };
            }

            foreach (var innerEx in innerExceptions)
            {
                foreach (string msg in innerEx.GetAllMessages())
                {
                    yield return msg;
                }
            }
        }
    }
}
