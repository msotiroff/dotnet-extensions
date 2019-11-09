using DotNetExtensions.Validation;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions.Common
{
    public static class StreamExtensions
    {
        public static string ReadAsString(this Stream stream, Encoding encoding = default)
        {
            stream.ThrowIfNull("Input stream cannot ne null.");

            encoding = (encoding == default)
                ? Encoding.UTF8
                : encoding;
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task<string> ToStringAsync(this Stream stream)
        {
            stream.ThrowIfNull("Input stream cannot ne null.");

            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);

                stream.RewindStream();

                ms.RewindStream();

                using (var sr = new StreamReader(ms))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }

        private static void RewindStream(this Stream stream)
        {
            stream.ThrowIfNull("Input stream cannot ne null.");

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }
}
