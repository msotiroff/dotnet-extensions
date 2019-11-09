using System;

namespace DotNetExtensions.Common
{
    public static class BooleanExtensions
    {
        public static bool And(this bool boolean, bool otherBoolean)
        {
            return boolean && otherBoolean;
        }

        public static bool Or(this bool boolean, bool otherBoolean)
        {
            return boolean || otherBoolean;
        }
    }
}
