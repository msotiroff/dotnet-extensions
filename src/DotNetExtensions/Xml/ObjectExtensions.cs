using DotNetExtensions.Validation;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetExtensions.Xml
{
    public static class ObjectExtensions
    {
        public static string ToXml<T>(this T serializable) where T : class
        {
            serializable.ThrowIfNull("Value for serialization cannot be null.");

            var serializer = new XmlSerializer(serializable.GetType());

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, serializable);

                    return sw.ToString();
                }
            }
        }

        public static string ToPlainXml<T>(this T serializable, bool indent = true)
            where T : class
        {
            serializable.ThrowIfNull("Value for serialization cannot be null.");

            var settings = new XmlWriterSettings
            {
                Indent = indent,
                OmitXmlDeclaration = true
            };

            var serializer = new XmlSerializer(serializable.GetType());

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, serializable);

                    return sw.ToString();
                }
            }
        }

        public static string SerializeToXml<T>(
            this T serializable, Action<XmlWriterSettings> settingsExpression) 
            where T : class
        {
            serializable.ThrowIfNull("Value for serialization cannot be null.");

            settingsExpression.ThrowIfNull("Xml writer settings cannot be null.");

            var serializer = new XmlSerializer(serializable.GetType());
            var settings = new XmlWriterSettings();

            settingsExpression.Invoke(settings);

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, serializable);

                    return sw.ToString();
                }
            }
        }

        public static string SerializeToPlainXml<T>(
            this T serializable, Action<XmlWriterSettings> settingsExpression)
            where T : class
        {
            serializable.ThrowIfNull("Value for serialization cannot be null.");

            settingsExpression.ThrowIfNull("Xml writer settings cannot be null.");

            var serializer = new XmlSerializer(serializable.GetType());
            var settings = new XmlWriterSettings();

            settingsExpression.Invoke(settings);

            settings.OmitXmlDeclaration = true;

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, serializable);

                    return sw.ToString();
                }
            }
        }

        public static string TrySerializeToXml<T>(this T serializable) where T : class
        {
            try
            {
                return serializable.ToXml();
            }
            catch
            {
                return default;
            }
        }

        public static string TrySerializeToPlainXml<T>(this T serializable, bool indent = true)
            where T : class
        {
            try
            {
                return serializable.ToPlainXml(indent);
            }
            catch
            {
                return default;
            }
        }

        public static string TrySerializeToXml<T>(
            this T serializable, Action<XmlWriterSettings> settingsExpression)
            where T : class
        {
            try
            {
                return serializable.SerializeToXml(settingsExpression);
            }
            catch
            {
                return default;
            }
        }

        public static string TrySerializeToPlainXml<T>(
            this T serializable, Action<XmlWriterSettings> settingsExpression)
            where T : class
        {
            try
            {
                return serializable.SerializeToPlainXml(settingsExpression);
            }
            catch
            {
                return default;
            }
        }
    }
}
