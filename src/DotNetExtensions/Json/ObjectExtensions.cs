using DotNetExtensions.Validation;
using Newtonsoft.Json;
using System;

namespace DotNetExtensions.Json
{
    public static class ObjectExtensions
    {
        public static string ToJson<T>(
            this T input, Formatting formatting = Formatting.Indented)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = formatting
            };

            return JsonConvert.SerializeObject(input, settings);
        }

        public static string SerializeToJson<T>(
            this T input, Action<JsonSerializerSettings> settingsExpression)
        {
            settingsExpression.ThrowIfNull("Serialization settings cannot be null.");

            var settings = new JsonSerializerSettings();

            settingsExpression.Invoke(settings);

            return JsonConvert.SerializeObject(input, settings);
        }

        public static string TrySerializeToJson<T>(
            this T input, Formatting formatting = Formatting.Indented)
        {
            try
            {
                return input.ToJson(formatting);
            }
            catch
            {
                return default;
            }
        }

        public static string TrySerializeToJson<T>(
            this T input, Action<JsonSerializerSettings> settingsExpression)
        {
            try
            {
                return input.SerializeToJson(settingsExpression);
            }
            catch
            {
                return default;
            }
        }
    }
}
