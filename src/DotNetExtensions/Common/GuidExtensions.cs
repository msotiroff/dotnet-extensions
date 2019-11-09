using System;

namespace DotNetExtensions.Common
{
    public static class GuidExtensions
    {
        public static string ToDashlessString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
