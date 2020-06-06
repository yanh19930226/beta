using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resillience.Util.Files
{
    /// <summary>
    /// 文件参数
    /// </summary>
    public interface IFileParameter : IDisposable
    {
        /// <summary>
        /// 获取文件流
        /// </summary>
        Stream GetFileStream();

        /// <summary>
        /// 获取文件名称
        /// </summary>
        string GetFileName();

        /// <summary>
        /// 获取参数名
        /// </summary>
        string GetName();
    }
}
