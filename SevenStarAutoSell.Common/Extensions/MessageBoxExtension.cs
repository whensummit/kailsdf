using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenStarAutoSell.Common.Extensions
{
    /// <summary>
    /// MessageBox扩展方法
    /// </summary>
    public static class MessageBoxEx
    {
        public static void Alert(string message)
        {
            MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Confirm(string message, IWin32Window ownerForm = null)
        {
            if (ownerForm != null)
            {
                return MessageBox.Show(ownerForm, message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                       DialogResult.Yes;
            }
            else
            {
                return MessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                       DialogResult.Yes;
            }
        }
    }
}
