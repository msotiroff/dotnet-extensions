using DotNetExtensions.Validation;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetExtensions.Xml
{
    public static class StringExtensions
    {
        public static TInstance DeserializeFromXml<TInstance>(this string input)
            where TInstance : class
        {
            return (TInstance)input.DeserializeFromXml(typeof(TInstance));
        }

        public static object DeserializeFromXml(this string xml, Type type)
        {
            xml.ThrowIfNullOrWhiteSpace("Input XML cannot be empty.");

            type.ThrowIfNull("The input type cannot be empty.");

            var serializer = new XmlSerializer(type);

            using (var reader = new StringReader(xml))
            {
                return serializer.Deserialize(reader);
            }
        }

        public static TInstance TryDeserializeFromXml<TInstance>(this string input)
            where TInstance : class
        {
            try
            {
                return input.DeserializeFromXml<TInstance>();
            }
            catch
            {
                return default;
            }
        }

        public static object TryDeserializeFromXml(this string xml, Type type)
        {
            try
            {
                return xml.DeserializeFromXml(type);
            }
            catch
            {
                return default;
            }
        }

        public static void Validate(this string xmlInput)
        {
            try
            {
                xmlInput.DeserializeFromXml<XmlDocument>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The input string is not a valid XML.", ex);
            }
        }

        public static bool IsValidXml(this string xmlInput)
        {
            try
            {
                xmlInput.Validate();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
