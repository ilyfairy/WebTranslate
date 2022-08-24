using System;
using System.Collections.Generic;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate
{
    public class KeyCombination
    {
        public KeyModifiers Modifier { get; set; }
        public Keys Key { get; set; }

        public KeyCombination()
        {

        }
        public KeyCombination(KeyModifiers modifier, Keys key)
        {
            Modifier = modifier;
            Key = key;
        }
        public override string ToString()
        {
            HashSet<ValueType> keys = new(4);
            if (Modifier.HasFlag(KeyModifiers.Windows)) keys.Add(KeyModifiers.Windows);
            if (Modifier.HasFlag(KeyModifiers.Control)) keys.Add(KeyModifiers.Control);
            if (Modifier.HasFlag(KeyModifiers.Shift)) keys.Add(KeyModifiers.Shift);
            if (Modifier.HasFlag(KeyModifiers.Alt)) keys.Add(KeyModifiers.Alt);
            if (Key != Keys.None) keys.Add(Key);

            if (Modifier == KeyModifiers.None && Key == Keys.None) return "无";
            return string.Join(" + ", keys);
        }
    }

}
