using System.Runtime.InteropServices;

namespace Ilyfairy.Tools.WebTranslate
{
    public static class WinApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]

        public static extern IntPtr GetFocus();
    }
}
