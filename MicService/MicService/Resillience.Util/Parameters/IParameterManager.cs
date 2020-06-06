using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util.Parameters
{
    /// <summary>
    /// 参数管理器
    /// </summary>
    public interface IParameterManager
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        object GetValue(string name);
    }
}
