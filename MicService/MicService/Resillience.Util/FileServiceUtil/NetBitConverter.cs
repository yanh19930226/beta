using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    public class NetBitConverter
    {
        public static byte[] GetBytes(bool value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(char value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(double value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(float value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(int value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(long value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(short value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(uint value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(ulong value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static byte[] GetBytes(ushort value)
        {
            return ReverseBits(BitConverter.GetBytes(value));
        }

        public static bool ToBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(ReverseBits(value, startIndex, 1), startIndex);
        }

        public static char ToChar(byte[] value, int startIndex)
        {
            return BitConverter.ToChar(ReverseBits(value, startIndex, 2), startIndex);
        }

        public static double ToDouble(byte[] value, int startIndex)
        {
            return BitConverter.ToDouble(ReverseBits(value, startIndex, 8), startIndex);
        }

        public static short ToInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToInt16(ReverseBits(value, startIndex, 2), startIndex);
        }

        public static int ToInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToInt32(ReverseBits(value, startIndex, 4), startIndex);
        }

        public static long ToInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToInt64(ReverseBits(value, startIndex, 8), startIndex);
        }

        public static float ToSingle(byte[] value, int startIndex)
        {
            return BitConverter.ToSingle(ReverseBits(value, startIndex, 4), startIndex);
        }

        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt16(ReverseBits(value, startIndex, 2), startIndex);
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt32(ReverseBits(value, startIndex, 4), startIndex);
        }

        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt64(ReverseBits(value, startIndex, 8), startIndex);
        }

        //
        // 摘要:
        //     /// 如果字节序是小端则反转 ///
        private static byte[] ReverseBits(byte[] bytes, int startIndex, int valLength)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, startIndex, valLength);
            }
            return bytes;
        }

        private static byte[] ReverseBits(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}
