using SevenStarAutoSell.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Defs
{
    public struct KeyData
    {
        public static KeyData Create(string key, byte[] data)
        {
            KeyData kd = new KeyData();
            kd.Key = key;
            kd.Data = data;

            return kd;
        }

        public static KeyData Create(string key)
        {
            return Create(key,"");
        }

        public static KeyData Create(string key, string data)
        {
            return Create(key, Encoding.UTF8.GetBytes(data));
        }

        public static KeyData Create(string key, object data)
        {
            return Create(key, Encoding.UTF8.GetBytes(JsonUtil.Serialize(data)));
        }

        public string Key
        {
            get; set;
        }

        public byte[] Data
        {
            get; set;
        }

        public string DataString
        {
            get
            {
                return Encoding.UTF8.GetString(this.Data);
            }
        }
    }
}
