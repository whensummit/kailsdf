using SevenStarAutoSell.Common.Utils;
using System.Text;

namespace SevenStarAutoSell.Common.Defs
{
    /// <summary>
    /// “SessionId,CmdText,MQData”模式组装MQ数据
    /// </summary>
    public struct IdKeyData
    {
        public static IdKeyData Create(string id, string key)
        {
            return IdKeyData.Create(id, key, Encoding.UTF8.GetBytes(""));
        }

        public static IdKeyData Create(string id, string key, string data)
        {
            return IdKeyData.Create(id, key, Encoding.UTF8.GetBytes(data));
        }

        public static IdKeyData Create(string id, string key, byte[] data)
        {
            IdKeyData ikd = new IdKeyData()
            {
                Id = id,
                Key = key,
                Data = data
            };
            return ikd;
        }

        public static IdKeyData Create(string id, string key, object data)
        {
            return IdKeyData.Create(id, key, Encoding.UTF8.GetBytes(JsonUtil.Serialize(data)));
        }

        /// <summary>
        /// ClientId/ SessionId
        /// </summary>
        public string Id
        {
            get; set;
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
