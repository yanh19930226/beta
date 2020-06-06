using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resillience.Util
{
    /// <summary>
    /// 枚举 操作
    /// </summary>
    public static class Enum
    {
        public static string GetDescription(Type type, object member) => Reflection.GetDescription(type, GetName(type, member));

        /// <summary>
        /// 获取成员名
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可，范例：Enum1枚举有成员A=0，则传入Enum1.A或0，获取成员名"A"</param>
        public static string GetName(Type type, object member)
        {
            if (type == null)
                return string.Empty;
            if (member == null)
                return string.Empty;
            if (member is string)
                return member.ToString();
            if (type.GetTypeInfo().IsEnum == false)
                return string.Empty;
            return System.Enum.GetName(type, member);
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名或值，范例：Enum1枚举有成员A=0，则传入"A"或"0"获取 Enum1.A</param>
        public static TEnum Parse<TEnum>(object member)
        {
            var value = member.SafeString();
            if (value.IsNullOrEmpty())
            {
                if (typeof(TEnum).IsGenericType)
                    return default;
                throw new ArgumentNullException(nameof(member));
            }
            return (TEnum)System.Enum.Parse(Common.GetType<TEnum>(), value, true);
        }
    }
}
