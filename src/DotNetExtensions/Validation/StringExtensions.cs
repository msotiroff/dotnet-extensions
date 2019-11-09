using System;

namespace DotNetExtensions.Validation
{
    public static class StringExtensions
    {
        #region Throwing default exception
        public static void ThrowIfNullOrEmpty(this string str, string errorMessage = default)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return;
            }

            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Input string cannot be empty."
                : errorMessage;

            throw new ArgumentNullException(errorMessage);
        }

        public static string ReturnOrThrowIfNullOrEmpty(
            this string str, string message = null)
        {
            ThrowIfNullOrEmpty(str, message);

            return str;
        }

        public static void ThrowIfNullOrWhiteSpace(this string str, string errorMessage = default)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return;
            }

            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Input string cannot be empty or whitespace."
                : errorMessage;

            throw new ArgumentNullException(errorMessage);
        }

        public static string ReturnOrThrowIfNullOrWhiteSpace(
            this string str, string errorMessage = default)
        {
            str.ThrowIfNullOrWhiteSpace(errorMessage);

            return str;
        }
        #endregion

        #region Throwing custom exception
        public static void ThrowIfNullOrEmpty<TException>(
            this string str, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (!string.IsNullOrEmpty(str))
            {
                return;
            }

            throw exception;
        }

        public static string ReturnOrThrowIfNullOrEmpty<TException>(
            this string str, TException exception)
            where TException : Exception
        {
            str.ThrowIfNullOrEmpty(exception);

            return str;
        }

        public static void ThrowIfNullOrWhiteSpace<TException>(
            this string str, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (!string.IsNullOrWhiteSpace(str))
            {
                return;
            }

            throw exception;
        }

        public static string ReturnOrThrowIfNullOrWhiteSpace<TException>(
            this string str, TException exception)
            where TException : Exception
        {
            str.ThrowIfNullOrWhiteSpace(exception);

            return str;
        }
        #endregion
    }
}
