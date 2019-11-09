using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetExtensions.Common;

namespace DotNetExtensions.AspNetCore.Common
{
    public static class HttpHeaderDictionaryExtensions
    {
        private const string MatchGroupName = "token";
        private const string BearerTokenPattern = @"^Bearer\s(?<token>[A-Za-z0-9]+)$";
        private const string HttpAuthorizationHeaderName = "Authorization";

        public static bool HasAuthorizationHeader(this IHeaderDictionary headers)
        {
            return headers.ContainsKey(HttpAuthorizationHeaderName);
        }

        public static string ExtractBearerToken(this IHeaderDictionary headers)
        {
            var authHeaderValue = headers.ExtractAuthenticationToken();

            if (authHeaderValue.IsNullOrWhiteSpace())
            {
                return default;
            }

            var bearerToken = Regex.Match(authHeaderValue, BearerTokenPattern)
                ?.Groups[MatchGroupName]
                ?.Value;

            return bearerToken;
        }

        public static string ExtractAuthenticationToken(this IHeaderDictionary headers)
        {
            if (!headers.HasAuthorizationHeader())
            {
                return default;
            }
            
            return headers[HttpAuthorizationHeaderName].SingleOrDefault();
        }
    }
}
