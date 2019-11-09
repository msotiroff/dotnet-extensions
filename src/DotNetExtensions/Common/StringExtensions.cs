using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace DotNetExtensions.Common
{
    public static class StringExtensions
    {
        public static byte ToByte(this string input)
        {
            return byte.Parse(input);
        }

        public static short ToShort(this string input)
        {
            return short.Parse(input);
        }

        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        public static long ToLong(this string input)
        {
            return long.Parse(input);
        }

        public static BigInteger ToBigInteger(this string input)
        {
            return BigInteger.Parse(input);
        }

        public static float ToFloat(this string input)
        {
            return float.Parse(input);
        }

        public static double ToDouble(this string input)
        {
            return double.Parse(input);
        }

        public static decimal ToDecimal(this string input)
        {
            return decimal.Parse(input);
        }

        public static DateTime ToDateTime(this string input)
        {
            return DateTime.Parse(input, CultureInfo.InvariantCulture);
        }

        public static TimeSpan ToTimeSpan(this string input)
        {
            return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
        }

        public static IEnumerable<byte> ToBytes(this string input, Encoding encoding = default)
        {
            encoding = (encoding == default)
                ? Encoding.UTF8
                : encoding;
            var bytes = encoding.GetBytes(input);

            return bytes;
        }

        public static Stream ToStream(this string input, Encoding encoding = default)
        {
            var bytes = input.ToBytes(encoding);
            var stream = new MemoryStream(bytes.ToArray());

            return stream;
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
