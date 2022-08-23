using System;
using System.Collections.Generic;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate;

public static class Constants
{
    public const string GlobalHotKey = "Ilyfairy.Tools.WebTranslate.GlobalHotKey"; //全局热键标识
    public const string RegistryName = "WebTranslate"; //注册表启动项名
    public const int WM_HOTKEY = 0x0312; //热键消息
    public static readonly Uri GoogleTranslate = new("https://translate.google.cn/");
    public static readonly Uri BaiduTranslate = new("https://fanyi.baidu.com/");
}
