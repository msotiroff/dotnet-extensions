using Newtonsoft.Json;
using System;

namespace DotNetExtensions.Json
{
    public static class StringExtensions
    {
        public static T FromJson<T>(this string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }

        public static object FromJson(this string jsonInput)
        {
            return JsonConvert.DeserializeObject(jsonInput);
        }

        public static T TryDeserializeFromJson<T>(this string jsonInput)
        {
            try
            {
                return jsonInput.FromJson<T>();
            }
            catch
            {
                return default;
            }
        }

        public static object TryDeserializeFromJson(this string jsonInput)
        {
            try
            {
                return jsonInput.FromJson();
            }
            catch
            {
                return default;
            }
        }

        public static void Validate(this string jsonInput)
        {
            try
            {
                jsonInput.FromJson();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "The input string is not a valid JSON.", ex);
            }
        }

        public static bool IsValidJson(this string jsonInput)
        {
            try
            {
                jsonInput.Validate();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
