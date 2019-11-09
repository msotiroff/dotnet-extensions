namespace DotNetExtensions.Common
{
    public static class IntExtensions
    {
        private const int Zero = 0;
        private const int One = 1;

        public static bool IsZero(this int source)
        {
            return source == Zero;
        }

        public static bool IsOne(this int source)
        {
            return source == One;
        }

        public static bool IsPositive(this int source)
        {
            return source > Zero;
        }

        public static bool IsNegative(this int source)
        {
            return source < Zero;
        }

        public static string ToZeroLeadingString(this int source, int leadingZeros = One)
        {
            var format = new string('0', leadingZeros + 1);

            return source.ToString(format);
        }
    }
}
