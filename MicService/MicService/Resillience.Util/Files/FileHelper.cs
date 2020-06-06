using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Resillience.Util.Files
{
    /// <summary>
    /// 文件操作辅助类
    /// </summary>
    public static partial class FileHelper
    {

        /// <summary>
        /// 流转换成字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符串编码</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="isCloseStream">读取完成是否释放流，默认为true</param>
        /// <returns></returns>
        public static string ToString(Stream stream, Encoding encoding = null, int bufferSize = 1024 * 2,
            bool isCloseStream = true)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (stream.CanRead == false)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var result = reader.ReadToEnd();
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }
        }
        /// <summary>
        /// 流转换成字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符串编码</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="isCloseStream">读取完成是否释放流，默认为true</param>
        /// <returns></returns>
        public static async Task<string> ToStringAsync(Stream stream, Encoding encoding = null,
            int bufferSize = 1024 * 2,
            bool isCloseStream = true)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (stream.CanRead == false)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(stream, encoding, true, bufferSize, !isCloseStream))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var result = await reader.ReadToEndAsync();
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }
        }
        /// <summary>
        /// 流转换成字节流
        /// </summary>
        /// <param name="stream">流</param>
        public static byte[] ToBytes(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 流转换成字节流
        /// </summary>
        /// <param name="stream">流</param>
        public static async Task<byte[]> ToBytesAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 将文件读取到字节流中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static byte[] ReadToBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return ReadToBytes(new FileInfo(filePath));
        }
        /// <summary>
        /// 将文件读取到字节流中
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns></returns>
        public static byte[] ReadToBytes(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            int fileSize = (int)fileInfo.Length;
            using (var reader = new BinaryReader(fileInfo.Open(FileMode.Open)))
            {
                return reader.ReadBytes(fileSize);
            }
        }
    }

    /// <summary>
    /// 文件写入类型
    /// </summary>
    public enum WriteType
    {
        /// <summary>
        /// 追加
        /// </summary>
        Append = 1,
        /// <summary>
        /// 覆盖
        /// </summary>
        Covered = 2
    }
}
