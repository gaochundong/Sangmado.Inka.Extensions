using System;
using System.ComponentModel;
using System.Reflection;

namespace RockStone.Inka.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue)
          where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            // try to find a DescriptionAttribute for a potential friendly name
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null)
            {
                if (memberInfo.Length > 0)
                {
                    object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs != null)
                    {
                        if (attrs.Length > 0)
                        {
                            return ((DescriptionAttribute)attrs[0]).Description;
                        }
                    }
                }
            }

            // we have no description attribute, just return the ToString
            return enumerationValue.ToString();
        }
    }
}
