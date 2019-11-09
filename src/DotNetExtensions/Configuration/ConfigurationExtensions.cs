using DotNetExtensions.Common;
using DotNetExtensions.Validation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetExtensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureStaticClass(
            this IConfiguration configuration, Type classType, string sectionName = default)
        {
            configuration.ThrowIfNull("The passed configuration cannot be null.");

            classType.ThrowIfNull("The passed class type cannot be null.");

            classType.IsStatic().ThrowIfFalse("Expected a static class.");

            if (sectionName.IsNullOrWhiteSpace())
            {
                sectionName = classType.Name;
            }

            var runtimeConstants = configuration
                .GetSection(sectionName)
                .GetChildren()
                .Select(item => new KeyValuePair<string, object>(item.Key, item.Value))
                .ToDictionary(x => x.Key, x => x.Value)
                .ForEach(kvp => SetMemberValue(kvp, classType));
        }

        private static void SetMemberValue(
            KeyValuePair<string, object> kvp, Type classType)
        {
            var propertyName = kvp.Key;
            var objectValue = kvp.Value;
            var property = classType.GetProperty(propertyName);

            if (property.IsNull())
            {
                throw new ArgumentException(
                    $"Could not set value. " +
                    $"{classType.FullName} does not contain a member, " +
                    $"named {propertyName}.");
            }

            var convertedValue = Convert.ChangeType(
                objectValue, property.PropertyType);

            property.SetValue(null, convertedValue);
        }
    }
}
