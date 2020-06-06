using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util
{
    public static class Common
    {
        #region GetType(获取类型)

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>() => GetType(typeof(T));

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type) => Nullable.GetUnderlyingType(type) ?? type;

        #endregion
    }
}
