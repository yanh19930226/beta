using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resillience.Util
{
    public static class Reflection
    {
        /// <summary>
        /// 获取类型成员描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription(Type type, string memberName)
        {
            if (type == null)
                return string.Empty;
            return memberName.IsNullOrEmpty()
                ? string.Empty
                : GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
        }
        /// <summary>
        /// 获取类型成员描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <param name="member">成员</param>
        public static string GetDescription(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            return member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute
                ? attribute.Description
                : member.Name;
        }
    }
}
