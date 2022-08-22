using System;
using System.Collections.Generic;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate;

public class WindowConfig
{
    public KeyCombination GlobalHotKey { get; set; }
    public bool AutoHide { get; set; }
    public bool TopMost { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public WindowConfig Clone()
    {
        var clone = (WindowConfig)this.MemberwiseClone();
        clone.GlobalHotKey = new KeyCombination();
        clone.GlobalHotKey.Modifier = GlobalHotKey.Modifier;
        clone.GlobalHotKey.Key = GlobalHotKey.Key;
        return clone;
    }

}
