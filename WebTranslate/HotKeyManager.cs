using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ilyfairy.Tools.WebTranslate;

public class HotKeyManager
{
    public IntPtr Hwnd { get; set; }
    public HotKeyManager(IntPtr hWnd)
    {
        Hwnd = hWnd;
    }

    private readonly Dictionary<int, Action> map = new();

    public bool Add(string id, ControlKeys modifyKey, Keys key, Action callback)
    {
        int hash = id.GetHashCode();
        bool isReg = RegisterHotKey(Hwnd, hash, modifyKey, key);
        if (isReg)
        {
            map[hash] = callback;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(string id)
    {
        map.Remove(id.GetHashCode());
    }

    public void Loop(int id)
    {
        if (map.TryGetValue(id, out Action? action))
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// 注册热键
    /// </summary>
    /// <param name="hWnd">窗口句柄，一般使用Handle属性 </param>
    /// <param name="id">区别热键的ID号,这个可以随便写，只是用来区分不同热键</param>
    /// <param name="fsModifiers">组合键</param>
    /// <param name="vk"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, ControlKeys fsModifiers, Keys vk);

    /// <summary>
    /// 注销热键 
    /// </summary>
    /// <param name="hWnd">窗口句柄</param>
    /// <param name="id">键标识</param>
    /// <returns></returns>
    [DllImport("user32.dll")] //导入WinAPI 
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}

public enum ControlKeys
{
    None = 0,
    Alt = 1,
    Control = 2,
    Shift = 4,
    Windows = 8
}