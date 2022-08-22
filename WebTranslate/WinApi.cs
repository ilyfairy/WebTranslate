using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate;

public static class WinApi
{
    [DllImport("user32.dll", EntryPoint = "GetAsyncKeyState")]
    private static extern short _GetAsyncKeyState(Keys vKey);

    /// <summary>
    /// 获取按键是否按下
    /// </summary>
    /// <param name="vKey"></param>
    /// <returns></returns>
    public static bool IsKeyDown(Keys vKey)
    {
        var v = _GetAsyncKeyState(vKey) & 0x8000;
        return v != 0;
    }

    /// <summary>
    /// 获取按键是否 "长按" (这次按键按下且上次调用时按键也按下)
    /// </summary>
    /// <param name="vKey"></param>
    /// <returns></returns>
    public static bool IsKeyPress(Keys vKey)
    {
        var v = _GetAsyncKeyState(vKey) & 0x0001;
        return v != 0;
    }
}
