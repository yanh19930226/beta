using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    public static class Convert
    {
        //
        // 摘要:
        //     /// ChangeType ///
        public static T ChangeType<T>(string str, T defVal = default(T))
        {
            Type typeFromHandle = typeof(T);
            return (T)ChangeType(str, typeFromHandle, defVal);
        }

        //
        // 摘要:
        //     /// 返回一个指定类型的对象，该对象的值等效于指定的对象。 ///
        public static object ChangeType(object value, Type conversionType, object defVal = null)
        {
            TypeInfo typeInfo = conversionType.GetTypeInfo();
            try
            {
                if (!(value is string))
                {
                    throw new NotSupportedException("不支持的类型");
                }
                string text = (string)value;
                if (conversionType == typeof(string))
                {
                    return text;
                }
                if (typeInfo.IsEnum)
                {
                    return Enum.Parse(conversionType, text, ignoreCase: true);
                }
                if (!typeInfo.IsValueType)
                {
                    throw new NotSupportedException("不支持的类型");
                }
                return System.Convert.ChangeType(text, conversionType, null);
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch
            {
                return (defVal == null && typeInfo.IsValueType) ? Activator.CreateInstance(conversionType) : defVal;
            }
        }

        //
        // 摘要:
        //     /// 将字节数组转换为Hex格式 ///
        //
        // 参数:
        //   bys:
        public static string ToHexString(byte[] bys)
        {
            if (bys == null)
            {
                throw new ArgumentNullException("bys");
            }
            StringBuilder stringBuilder = new StringBuilder(bys.Length * 2);
            for (int i = 0; i < bys.Length; i++)
            {
                byte b = bys[i];
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        //
        // 摘要:
        //     /// HexString转换为bytes，转换失败会抛出ArgumentNullException异常 ///
        public static byte[] FromHexString(string hexStr)
        {
            if (hexStr == null || hexStr.Length < 2 || hexStr.Length % 2 != 0)
            {
                throw new ArgumentNullException("hexStr");
            }
            byte[] array = new byte[hexStr.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                string s = hexStr.Substring(i * 2, 2);
                array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return array;
        }

        //
        // 摘要:
        //     /// 转义加密后的字符串为常规字符串 /// 此方法适合要求加密并且高性能不要求加密强度的场景（例如防破解的字符串加密） ///
        //
        // 参数:
        //   str:
        //     待加解密的字符串
        //
        //   pwd:
        //     密码
        public static string Xor(string str, string pwd)
        {
            return CrackProtection.Xor(str, pwd);
        }

        public static string Xor(string str)
        {
            return CrackProtection.Xor(str);
        }

        //
        // 摘要:
        //     /// 转换为代码字符串 ///
        public static string ToCodeString(string str)
        {
            StringBuilder stringBuilder = new StringBuilder(str.Length * 6);
            foreach (char c in str)
            {
                short num = (short)c;
                if (num >= 32 && num <= 126)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("\\u" + ((short)c).ToString("x4"));
                }
            }
            return stringBuilder.ToString();
        }
    }
}
