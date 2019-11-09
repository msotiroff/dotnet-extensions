using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotNetExtensions.Validation
{
    public static class ObjectExtensions
    {
        #region Throwing default exception
        public static void ThrowIfNull<T>(this T source, string errorMessage = default)
            where T : class
        {
            if (source != null)
            {
                return;
            }

            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                    ? "Value cannot be null."
                    : errorMessage;

            throw new ArgumentNullException(errorMessage);
        }

        public static void ThrowIfAnyNull<T>(
            this IEnumerable<T> source, string errorMessage = default) 
            where T : class
        {
            ThrowIfNull(source, errorMessage);

            foreach (var obj in source)
            {
                ThrowIfNull(obj, errorMessage);
            }
        }

        public static T ReturnOrThrowIfNull<T>(this T source, string errorMessage = default)
            where T : class
        {
            ThrowIfNull(source, errorMessage);

            return source;
        }

        public static void ThrowIfNullOrAnyMemberNull<T>(
            this T source, string errorMessage = default)
            where T : class
        {
            ThrowIfNull(source, errorMessage);

            var members = source
                .GetType()
                .GetProperties()
                .Select(pi => pi.GetValue(source))
                .ToList();

            ThrowIfAnyNull(members, errorMessage);
        }

        public static void ThrowIfDefaultValue<T>(this T source, string errorMessage = default)
        {
            errorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Velue cannot be the default one."
                : errorMessage;

            if (source == null)
            {
                throw new ArgumentException(errorMessage);
            };

            var sourceType = source.GetType();

            if (!sourceType.IsValueType)
            {
                return;
            }

            var defaultValue = Activator.CreateInstance(sourceType);
            
            if (source.Equals(defaultValue))
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static void ValidateByAttributes(this object source)
        {
            Validator.ValidateObject(source, new ValidationContext(source), true);
        }
        #endregion

        #region Throwing custom exception
        public static void ThrowIfNull<T, TException>(this T source, TException exception)
            where T : class
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (source != null)
            {
                return;
            }

            throw exception;
        }

        public static void ThrowIfAnyNull<T, TException>(
            this IEnumerable<T> source, TException exception)
            where T : class
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            source.ThrowIfNull(exception);

            foreach (var item in source)
            {
                item.ThrowIfNull(exception);
            }
        }

        public static T ReturnOrThrowIfNull<T, TException>(
            this T source, TException exception)
            where T : class
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            source.ThrowIfNull(exception);

            return source;
        }

        public static void ThrowIfNullOrAnyMemberNull<T, TException>(
            this T source, TException exception)
            where T : class
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            source.ThrowIfNull(exception);

            var members = source
                .GetType()
                .GetProperties()
                .Select(pi => pi.GetValue(source))
                .ToList();

            ThrowIfAnyNull(members, exception);
        }

        public static void ThrowIfDefaultValue<T, TException>(
            this T source, TException exception)
            where TException : Exception
        {
            exception.ThrowIfNull("The passed exception cannot be null.");

            if (source == null)
            {
                throw exception;
            };

            var sourceType = source.GetType();

            if (!sourceType.IsValueType)
            {
                return;
            }

            var defaultValue = Activator.CreateInstance(sourceType);

            if (source.Equals(defaultValue))
            {
                throw exception;
            }
        }
        #endregion
    }
}
