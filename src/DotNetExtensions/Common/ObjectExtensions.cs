using DotNetExtensions.Json;
using DotNetExtensions.Validation;
using DotNetExtensions.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNetExtensions.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns whether the given object is null. NEVER use over 'dynamic'.
        /// </summary>
        public static bool IsNull(this object input)
        {
            return input == null;
        }

        /// <summary>
        /// Returns whether the given object is not null. NEVER use over 'dynamic'.
        /// </summary>
        public static bool IsNotNull(this object input)
        {
            return input != null;
        }

        public static T TryClone<T>(this T source) where T : class
        {
            try
            {
                return source.Clone();
            }
            catch
            {
                return default;
            }
        }

        public static T Clone<T>(this T source) where T : class
        {
            source.ThrowIfNull("Source object is null. Could not clone it.");

            var isSerializable = source
                .GetType()
                .IsDefined(typeof(SerializableAttribute), true);

            if (!isSerializable)
            {
                return source.CloneNonSerializeble();
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();

                    formatter.Serialize(ms, source);

                    ms.Position = 0;

                    return (T)formatter.Deserialize(ms);
                }
            }
            catch
            {
                return source.CloneUsingXmlSerializer();
            }
        }

        public static bool HasMinimumNotDefaultMembers<T>(
            this T source,
            int minNotDefaultMembers,
            bool treatEmptyCollectionsAsDefaultValue = false)
            where T : class
        {
            if (minNotDefaultMembers < 1)
            {
                return true;
            }

            var properties = source.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                var memberType = propertyInfo.PropertyType;
                var actualValue = Convert.ChangeType(propertyInfo.GetValue(source), memberType);
                var defaultValue = memberType.IsValueType
                    ? Convert.ChangeType(Activator.CreateInstance(memberType), memberType)
                    : null;

                if (actualValue is string && string.IsNullOrWhiteSpace((string)actualValue))
                {
                    actualValue = default(string);
                }

                var areEquals = memberType.IsValueType
                    ? (actualValue.Equals(defaultValue))
                    : (actualValue == defaultValue);

                if ((memberType.IsAssignableFrom(typeof(IEnumerable<T>)) ||
                        memberType.IsAssignableFrom(typeof(T[]))) &&
                    treatEmptyCollectionsAsDefaultValue)
                {
                    var castedCollection = actualValue as IEnumerable<T>
                        ?? actualValue as T[];
                    areEquals = actualValue.Equals(defaultValue) ||
                        castedCollection.Count() == 0;
                }

                if (!areEquals)
                {
                    minNotDefaultMembers--;
                }

                if (minNotDefaultMembers == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void ValidateByAttributes(this object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                obj, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new InvalidOperationException(string.Join(
                    Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage)));
            }
        }

        public static IEnumerable<ValidationResult> GetValidationResults(this object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return validationResults;
        }

        private static T CloneNonSerializeble<T>(this T source)
        {
            var serialized = source.ToJson();
            var clone = serialized.FromJson<T>();

            return clone;
        }

        private static T CloneUsingXmlSerializer<T>(this T source) where T : class
        {
            var serialized = source.ToXml();
            var clone = serialized.DeserializeFromXml<T>();

            return clone;
        }
    }
}
