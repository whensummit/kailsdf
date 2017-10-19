using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Defs
{
    /// <summary>
    /// Action参数为单值类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ValueTypeParam<T>
    {
        public T Value { get; set; }

        public ValueTypeParam()
        {
            Value = default(T);
        }

        public ValueTypeParam(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 承载平台 扫水或者
        /// </summary>
        public BetPlatformEnum? Platform { get; set; }
    }
}
