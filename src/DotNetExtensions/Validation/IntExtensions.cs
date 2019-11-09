using System;

namespace DotNetExtensions.Validation
{
    public static class IntExtensions
    {
        #region Throwing default exception
        public static void ThrowIfNotPositive(this int input, string errorMessage = default)
        {
            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Value must be a positive integer."
                : errorMessage;

            if (input <= 0)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static void ThrowIfPositive(this int input, string errorMessage = default)
        {
            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Value cannot be positive."
                : errorMessage;

            if (input > 0)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static int ReturnOrThrowIfNotPositive(this int input, string errorMessage = default)
        {
            input.ThrowIfNotPositive(errorMessage);

            return input;
        }

        public static int ReturnOrThrowIfPositive(this int input, string errorMessage = default)
        {
            input.ThrowIfPositive(errorMessage);

            return input;
        }

        public static void ThrowIfNotNegative(this int input, string errorMessage = default)
        {
            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Value must be a negative integer."
                : errorMessage;

            if (input >= 0)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static void ThrowIfNegative(this int input, string errorMessage = default)
        {
            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Value cannot be negative."
                : errorMessage;

            if (input < 0)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static int ReturnOrThrowIfNotNegative(this int input, string errorMessage = default)
        {
            input.ThrowIfNotNegative(errorMessage);

            return input;
        }

        public static int ReturnOrThrowIfNegative(this int input, string errorMessage = default)
        {
            input.ThrowIfNegative(errorMessage);

            return input;
        }
        #endregion

        #region Throwing custom exception
        public static void ThrowIfNotPositive<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (input <= 0)
            {
                throw exception;
            }
        }

        public static void ThrowIfPositive<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (input > 0)
            {
                throw exception;
            }
        }

        public static int ReturnOrThrowIfNotPositive<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            input.ThrowIfNotPositive(exception);

            return input;
        }

        public static int ReturnOrThrowIfPositive<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            input.ThrowIfPositive(exception);

            return input;
        }

        public static void ThrowIfNotNegative<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (input >= 0)
            {
                throw exception;
            }
        }

        public static void ThrowIfNegative<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (input < 0)
            {
                throw exception;
            }
        }

        public static int ReturnOrThrowIfNotNegative<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            input.ThrowIfNotNegative(exception);

            return input;
        }

        public static int ReturnOrThrowIfNegative<TException>(
            this int input, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            input.ThrowIfNegative(exception);

            return input;
        }
        #endregion
    }
}
