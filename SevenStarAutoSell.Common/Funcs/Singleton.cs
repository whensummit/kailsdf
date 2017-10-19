using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Funcs
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance = new T();

        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
