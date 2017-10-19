using SevenStarAutoSell.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Crawl.Common
{
    /// <summary>
    /// 初始化当前客户端支持的扫水平台
    /// </summary>
    public class InitCurrentPlatforms
    {
        /// <summary>
        /// 初始化平台
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            DirectoryInfo dir = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}Plugs\\");
            var fileList= dir.GetFiles("SevenStarAutoSell.Business.Web.*.dll");          
            if (fileList.Count() > 0)
            {
                foreach (var file in fileList)
                {
                    //加载程序集
                    Assembly assembly = Assembly.LoadFile(file.FullName);
                    var adapterList = assembly.GetTypes().Where(q => q.GetInterfaces().Contains(typeof(IWebBetAdapter)));
                    if (adapterList.Any())
                    {
                        var adapter = adapterList.FirstOrDefault();
                        IWebBetAdapter webadapter = (IWebBetAdapter)assembly.CreateInstance(adapter.FullName, false);

                        if (!PublicData.CurrentPlatforms.ContainsKey(webadapter.Platform))
                        {
                            PublicData.CurrentPlatforms.TryAdd(webadapter.Platform, webadapter);
                        }                      
                    }                    
                }                         
            }

            return true;
        }

       

    }
}
