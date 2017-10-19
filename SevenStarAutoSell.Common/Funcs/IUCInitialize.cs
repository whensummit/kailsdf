using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Funcs
{
    /// <summary>
    /// 统一自定义用户控件的初始化接口
    /// </summary>
    public interface IUCInitialize
    {
        /// <summary>
        /// 安装（初始化控件）
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载控件
        /// </summary>
        void Uninstall();
    }
}
