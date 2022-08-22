using System;
using System.Collections.Generic;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate;

public class Constants
{
    public const string GlobalHotKey = "Ilyfairy.Tools.WebTranslate.GlobalHotKey"; //全局热键标识
    public const string RegistryName = "WebTranslate"; //注册表启动项名
    public const int WM_HOTKEY = 0x0312; //热键消息
    public const int WH_KEYBOARD = 2; //键盘Hook
    public const int WM_MOVE = 0x0003;
    public const int WM_SYSCOMMAND = 0x112;
    public const int SC_MOVE = 0xF012;

    public const int WM_SYSKEYDOWN = 0x0104;
    public const int WM_KEYDOWN = 0x0100;
    public const int WM_SYSKEYUP = 0x0105;
    public const int WM_KEYUP = 0x0101;
    public const int WM_SYSCHAR = 0x0106;
    public const int WM_CHAR = 0x0102;
    public const int WM_COMMAND = 0x0111;
}
