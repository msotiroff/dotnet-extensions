using System;

namespace DotNetExtensions.Validation
{
    public static class BooleanExtensions
    {
        #region Throwing default exception
        public static void ThrowIfTrue(this bool boolean, string errorMessage = default)
        {
            if (boolean == true)
            {
                errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                    ? "The condition cannot be true."
                    : errorMessage;

                throw new ArgumentException(errorMessage);
            }
        }

        public static void ThrowIfFalse(this bool boolean, string errorMessage = default)
        {
            if (boolean == false)
            {
                errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                    ? "The condition cannot be false."
                    : errorMessage;

                throw new ArgumentException(errorMessage);
            }
        }
        #endregion

        #region Throwing default exception
        public static void ThrowIfTrue<TException>(
            this bool boolean, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (boolean == true)
            {
                throw exception;
            }
        }

        public static void ThrowIfFalse<TException>(
            this bool boolean, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (boolean == false)
            {
                throw exception;
            }
        }
        #endregion
    }
}
