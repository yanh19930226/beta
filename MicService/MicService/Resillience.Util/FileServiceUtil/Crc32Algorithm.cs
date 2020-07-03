using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    public class Crc32Algorithm : HashAlgorithm
    {
        private static readonly uint[] Crc32Table;

        private uint _crcResult = uint.MaxValue;

        static Crc32Algorithm()
        {
            Crc32Table = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                uint num = (uint)i;
                for (int num2 = 8; num2 > 0; num2--)
                {
                    num = (uint)(((num & 1) != 1) ? ((int)(num >> 1)) : ((int)(num >> 1) ^ -306674912));
                }
                Crc32Table[i] = num;
            }
        }

        public void Reset()
        {
            _crcResult = uint.MaxValue;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < ibStart + cbSize; i++)
            {
                uint num = _crcResult & 0xFF;
                num ^= array[i];
                _crcResult >>= 8;
                _crcResult ^= Crc32Table[num];
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] obj = new byte[4];
            _crcResult ^= uint.MaxValue;
            obj[0] = (byte)((_crcResult >> 24) & 0xFF);
            obj[1] = (byte)((_crcResult >> 16) & 0xFF);
            obj[2] = (byte)((_crcResult >> 8) & 0xFF);
            obj[3] = (byte)(_crcResult & 0xFF);
            return obj;
        }

        public override void Initialize()
        {
            Reset();
        }
    }
}
