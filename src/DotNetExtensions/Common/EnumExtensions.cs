using System;

namespace DotNetExtensions.Common
{
    public static class EnumExtensions
    {
        public static string GetValueAsString(this Enum value)
        {
            EnsureEnum(value);

            var fieldInfo = value.GetType().GetField(value.ToString());
            var intValue = (int)fieldInfo.GetValue(value);

            return intValue.ToString();
        }

        private static void EnsureEnum(Enum value)
        {
            if (!value.GetType().IsEnum)
            {
                throw new ArgumentException("This operation can be performed only over enums.");
            }
        }
    }
}
