using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resillience.Util.FileServiceUtil
{
    public class FileUtil : Object
    {
        public static Boolean TryDelete(String filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static String GetSha1(String filePath)
        {
            FileStream val = File.OpenRead(filePath);
            try
            {
                return HashUtil.Sha1(val);
            }
            finally
            {
                if (val != null)
                {
                    val.Dispose();
                }
            }
        }

        public FileUtil()
        {

        }
    }
}
