using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetExtensions.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime PreviousDay(this DateTime dateTime)
        {
            return dateTime.AddDays(-1);
        }

        public static DateTime NextDay(this DateTime dateTime)
        {
            return dateTime.AddDays(1);
        }

        public static bool IsBetweenIncluding(
            this DateTime dateTime,
            DateTime? startDate,
            DateTime? endDate)
        {
            if (!(startDate.HasValue || endDate.HasValue))
            {
                return false;
            }

            if (startDate.Value.Date > endDate.Value.Date)
            {
                throw new InvalidOperationException("End date should be after start date.");
            }

            return dateTime.Date >= startDate.Value.Date &&
                dateTime.Date <= endDate.Value.Date;
        }
    }
}
