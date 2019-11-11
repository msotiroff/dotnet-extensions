using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetExtensions.Cryptography
{
    public static class StringExtensions
    {
        public static string ToSHA512Hash(this string plainText)
        {
            using (var sha = SHA512.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                return hash.ToHex(upperCase: false);
            }
        }

        public static string ToSHA256Hash(this string plainText)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                return hash.ToHex(false);
            }
        }

        public static string Encrypt(this string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                return Convert.ToBase64String(aes.Encrypt(plainText, key, new byte[16]));
            }
        }

        public static string Decrypt(this string cipherText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                return aes.Decrypt(key, new byte[16], Convert.FromBase64String(cipherText));
            }
        }

        private static string Decrypt(
            this SymmetricAlgorithm algorithm,
            string key,
            byte[] iv,
            byte[] buffer)
        {
            algorithm.Key = Encoding.UTF8.GetBytes(key);
            algorithm.IV = iv;
            ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream(
                    memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        private static byte[] Encrypt(
            this SymmetricAlgorithm algorithm,
            string plainText,
            string key,
            byte[] iv)
        {
            algorithm.Key = Encoding.UTF8.GetBytes(key);
            algorithm.IV = iv;

            var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(
                    memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        private static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            }

            return result.ToString();
        }
    }
}
