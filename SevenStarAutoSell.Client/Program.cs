using System;
using System.Threading;
using System.Windows.Forms;

namespace SevenStarAutoSell.Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 跨线程调用UI控件
            Control.CheckForIllegalCrossThreadCalls = false;

            // 程序异常拦截
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());//MainForm
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception.Source.Equals("FlexCell", StringComparison.CurrentCultureIgnoreCase))
            {
                //LogUtil.Warn("ThreadException - FlexCell：" + e.Exception.ToString());
            }
            else
            {
                //LogUtil.Error("ThreadException：" + e.Exception.ToString());
                //MessageBoxEx.Alert(e.Exception.Message);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //LogUtil.Error("UnhandledException：" + e.ExceptionObject.ToString());
            //MessageBoxEx.Alert(e.ExceptionObject.ToString());
        }


    }
}
