using System;

namespace DotNetExtensions.Common
{
    public static class TypeExtensions
    {
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract.And(type.IsSealed);
        }
    }
}
