using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util
{
    class Conv
    {
        #region To(通用泛型转换)

        /// <summary>
        /// 通用泛型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="input">输入值</param>
        public static T To<T>(object input)
        {
            if (input == null)
                return default;
            if (input is string && string.IsNullOrWhiteSpace(input.ToString()))
                return default;

            var type = Common.GetType<T>();
            var typeName = type.Name.ToLower();
            try
            {
                if (typeName == "string")
                    return (T)(object)input.ToString();
                if (typeName == "guid")
                    return (T)(object)new Guid(input.ToString());
                if (type.IsEnum)
                    return Enum.Parse<T>(input);
                if (input is IConvertible)
                    return (T)System.Convert.ChangeType(input, type);
                return (T)input;
            }
            catch
            {
                return default;
            }
        }

        #endregion
    }
}
