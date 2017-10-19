using Framework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.DataAccess
{
    public static class ParameterEx
    {
        public static Parameter AddMore(this Parameter parameter, string name, object value)
        {
            parameter.Add(name, value);
            return parameter;
        }
    }
}
