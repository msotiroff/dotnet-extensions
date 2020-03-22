using System;

namespace DotNetExtensions.Common
{
    public static class LongExtensions
    {
        private const int Zero = 0;
        private const int One = 1;

        public static bool IsZero(this long source)
        {
            return source == Zero;
        }

        public static bool IsOne(this long source)
        {
            return source == One;
        }

        public static bool IsPositive(this long source)
        {
            return source > Zero;
        }

        public static bool IsNegative(this long source)
        {
            return source < Zero;
        }

        public static string ToZeroLeadingString(this long source, int leadingZeros = One)
        {
            var format = new string('0', leadingZeros + 1);

            return source.ToString(format);
        }

        public static DateTime FromUnixTime(this long value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value);
        }
    }
}
