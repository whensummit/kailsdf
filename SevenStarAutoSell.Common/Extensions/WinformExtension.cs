using System.Windows.Forms;

namespace SevenStarAutoSell.Common.Extensions
{
    public static class WinformExtension
    {
        public static void ActivateNormal(this Form form)
        {
            form.Show();
            form.Activate();
            form.WindowState = FormWindowState.Normal;
        }
    }
}
