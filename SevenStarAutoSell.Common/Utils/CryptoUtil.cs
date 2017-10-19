using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SevenStarAutoSell.Common.Utils
{
    public static class CryptoUtil
    {
        private static string _defaultDesKey;

        private static char[] _allchars;

        static CryptoUtil()
        {
            CryptoUtil._defaultDesKey = "95279527";
            CryptoUtil._allchars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '!', '@', '#', '$', '%', '\u005E', '&', '*', '(', ')' };
        }

        public static string DesDecrypt(string text)
        {
            return CryptoUtil.DesDecrypt(text, CryptoUtil._defaultDesKey);
        }

        public static string DesDecrypt(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider()
            {
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(key)
            };
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            byte[] inputByteArray = new byte[text.Length / 2];
            for (int x = 0; x < text.Length / 2; x++)
            {
                int i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            cs.Write(inputByteArray, 0, (int)inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string DesEncrypt(string text)
        {
            return CryptoUtil.DesEncrypt(text, CryptoUtil._defaultDesKey);
        }

        public static string DesEncrypt(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider()
            {
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(key)
            };
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            cryptoStream.Write(inputByteArray, 0, (int)inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            for (int i = 0; i < (int)array.Length; i++)
            {
                ret.AppendFormat("{0:X2}", array[i]);
            }
            return ret.ToString();
        }

        public static string GetDesKey()
        {
            string key = "";
            Random r = new Random();
            for (int i = 0; i < 8; i++)
            {
                key = string.Concat(key, CryptoUtil._allchars[r.Next(72)].ToString());
            }
            return key;
        }

        public static string[] GetPublicPrivateKey()
        {
            RSACryptoServiceProvider crypto = new RSACryptoServiceProvider();
            return new string[] { crypto.ToXmlString(false), crypto.ToXmlString(true) };
        }

        public static string MD5(string text)
        {
            return Convert.ToBase64String((new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string RsaDecrypt(string text, string privateKey)
        {
            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
            rSACryptoServiceProvider.FromXmlString(privateKey);
            byte[] bytes = rSACryptoServiceProvider.Decrypt(Convert.FromBase64String(text), true);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string RsaEncrypt(string text, string publicKey)
        {
            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
            rSACryptoServiceProvider.FromXmlString(publicKey);
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(bytes, true));
        }
    }
}