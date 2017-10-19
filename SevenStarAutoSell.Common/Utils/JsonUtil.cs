using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Utils
{
    public static class JsonUtil
    {
        public static bool EqualJson(object objone, object objtwo)
        {
            if (objone == null && objtwo == null) return true;
            if (objone == null || objtwo == null) return false;
            return JsonConvert.SerializeObject(objone).Equals(JsonConvert.SerializeObject(objtwo));
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T Deserialize<T>(byte[] value)
        {
            return JsonConvert.DeserializeObject<T>(GetString(value));
        }

        public static object Deserialize(byte[] value, Type type)
        {
            return JsonConvert.DeserializeObject(GetString(value), type);
        }

        public static string GetString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        public static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
