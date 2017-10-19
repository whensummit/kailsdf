﻿using SevenStarAutoSell.CenterServer.Shell.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenStarAutoSell.CenterServer.Shell
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RouteConfig.Instance.RegisterRoutes();
            Application.Run(new FormMain());
          
        }
    }
}