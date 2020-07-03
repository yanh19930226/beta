using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    public static class HashUtil
    {
        public static uint Crc32(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            using (Crc32Algorithm crc32Algorithm = new Crc32Algorithm())
            {
                return NetBitConverter.ToUInt32(crc32Algorithm.ComputeHash(inputStream), 0);
            }
        }

        public static uint Crc32(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (Crc32Algorithm crc32Algorithm = new Crc32Algorithm())
            {
                return NetBitConverter.ToUInt32(crc32Algorithm.ComputeHash(bytes), 0);
            }
        }

        public static uint Crc32(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            return Crc32((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        public static string Md5(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            using (MD5 algorithm = MD5.Create())
            {
                return ComputeHash(algorithm, inputStream);
            }
        }

        public static string Md5(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (MD5 algorithm = MD5.Create())
            {
                return ComputeHash(algorithm, bytes);
            }
        }

        public static string Md5(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            return Md5((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        public static string Sha1(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            using (SHA1 algorithm = SHA1.Create())
            {
                return ComputeHash(algorithm, inputStream);
            }
        }

        public static string Sha1(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (SHA1 algorithm = SHA1.Create())
            {
                return ComputeHash(algorithm, bytes);
            }
        }

        public static string Sha1(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            return Sha1((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        public static string Sha256(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            using (SHA256 algorithm = SHA256.Create())
            {
                return ComputeHash(algorithm, inputStream);
            }
        }

        public static string Sha256(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (SHA256 algorithm = SHA256.Create())
            {
                return ComputeHash(algorithm, bytes);
            }
        }

        public static string Sha256(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            return Sha256((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        public static string Sha512(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            using (SHA512 algorithm = SHA512.Create())
            {
                return ComputeHash(algorithm, inputStream);
            }
        }

        public static string Sha512(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (SHA512 algorithm = SHA512.Create())
            {
                return ComputeHash(algorithm, bytes);
            }
        }

        public static string Sha512(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            return Sha512((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        private static string ComputeHash(HashAlgorithm algorithm, byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            return Convert.ToHexString(algorithm.ComputeHash(bytes));
        }

        private static string ComputeHash(HashAlgorithm algorithm, Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            return Convert.ToHexString(algorithm.ComputeHash(inputStream));
        }
    }
}
