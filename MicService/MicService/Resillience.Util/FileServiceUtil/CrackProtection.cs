using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    internal static class CrackProtection
    {
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
            char[] array = str.ToCharArray();
            char[] array2 = pwd.ToCharArray();
            int num = array.Length;
            int num2 = array2.Length;
            for (int i = 0; i < num; i++)
            {
                char c = array[i];
                array[i] = (char)(c ^ array2[i % num2]);
            }
            return new string(array);
        }

        public static string Xor(string str)
        {
            return Xor(str, "mondol");
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

        //
        // 摘要:
        //     /// 反射调用一个方法 ///
        public static object FastInvokeMethod(Type type, string methodName, Type[] paramTypes, params object[] parameters)
        {
            return type.GetTypeInfo().GetMethod(methodName, paramTypes).Invoke(null, parameters);
        }

        //
        // 摘要:
        //     /// 用于检测到破解后，破坏内存结构阻止程序运行 ///
        //
        // 参数:
        //   isTamper:
        //     是否已被篡改
        public static void Destroy(bool isTamper)
        {
        }
    }
}
