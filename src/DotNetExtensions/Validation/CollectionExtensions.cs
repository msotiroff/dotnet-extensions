using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetExtensions.Validation
{
    public static class CollectionExtensions
    {
        #region Throwing default exception
        public static void ThrowIfNullOrEmpty<T>(
            this IEnumerable<T> collection, string errorMessage = default)
        {
            collection.ThrowIfNull(errorMessage);

            if (collection.Count() == 0)
            {
                errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                    ? "Collection cannot be empty."
                    : errorMessage;

                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowIfAnyNull<T>(
            this IEnumerable<T> collection, string errorMessage = default)
            where T : class
        {
            collection.ThrowIfNull(errorMessage);

            foreach (var item in collection)
            {
                item.ThrowIfNull(errorMessage);
            }
        }

        public static void ThrowIfAllNull<T>(
            this IEnumerable<T> collection, string errorMessage = default) 
            where T : class
        {
            collection.ThrowIfNull(errorMessage);

            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "All items cannot be null."
                : errorMessage;

            if (collection.All(item => item == null))
            {
                throw new ArgumentNullException(errorMessage);
            }
        }
        #endregion

        #region Throwing custom exception
        public static void ThrowIfNullOrEmpty<T, TException>(
            this IEnumerable<T> collection, TException exception)
            where TException : Exception
        {
            collection.ThrowIfNull(exception);

            if (collection.Count() == 0)
            {
                throw exception;
            }
        }

        public static void ThrowIfAnyNull<T, TException>(
            this IEnumerable<T> collection, TException exception)
            where T : class
            where TException : Exception
        {
            collection.ThrowIfNull(exception);

            foreach (var item in collection)
            {
                item.ThrowIfNull(exception);
            }
        }

        public static void ThrowIfAllNull<T, TException>(
            this IEnumerable<T> collection, TException exception)
            where T : class
            where TException : Exception
        {
            collection.ThrowIfNull(exception);

            if (collection.All(item => item == null))
            {
                throw exception;
            }
        }
        #endregion
    }
}
