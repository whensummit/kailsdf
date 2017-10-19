using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Defs
{
    /// <summary>
    /// 未找到Session异常
    /// </summary>
    public class NotFindSessionException : Exception
    {
        public NotFindSessionException(string errorMessage)
            : base(errorMessage)
        { }
    }
}
