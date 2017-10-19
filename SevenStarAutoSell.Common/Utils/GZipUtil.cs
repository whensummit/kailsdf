using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SevenStarAutoSell.Common.Utils
{
    public class GZipUtil
    {
        public GZipUtil()
        {
        }

        public static byte[] Compress(byte[] data)
        {
            return GZipUtil.Compress(data, 0, (int)data.Length);
        }

        public static byte[] Compress(byte[] data, int index, int count)
        {
            byte[] array;
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress, true);
                gZipStream.Write(data, index, count);
                gZipStream.Close();
                array = ms.ToArray();
            }
            return array;
        }

        public static byte[] CompressString(string data)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(data);
            return GZipUtil.Compress(tmp, 0, (int)tmp.Length);
        }

        public static byte[] Decompress(byte[] data)
        {
            return GZipUtil.Decompress(data, 0, (int)data.Length);
        }

        public static byte[] Decompress(byte[] data, int index, int count)
        {
            byte[] array;
            using (MemoryStream tempMs = new MemoryStream())
            {
                using (MemoryStream ms = new MemoryStream(data, index, count))
                {
                    GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress, true);
                    gZipStream.CopyTo(tempMs);
                    gZipStream.Close();
                    array = tempMs.ToArray();
                }
            }
            return array;
        }

        public static string DecompressString(byte[] data)
        {
            byte[] tmp = GZipUtil.Decompress(data, 0, (int)data.Length);
            return Encoding.UTF8.GetString(tmp);
        }
    }
}
