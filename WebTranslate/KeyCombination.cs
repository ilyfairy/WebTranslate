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
            string key =
                (Modifier.HasFlag(KeyModifiers.Windows) ? "Win + " : "") +
                (Modifier.HasFlag(KeyModifiers.Control) ? "Ctrl + " : "") +
                (Modifier.HasFlag(KeyModifiers.Shift) ? "Shift + " : "") +
                (Modifier.HasFlag(KeyModifiers.Alt) ? "Alt + " : "") + Key.ToString();
            return key;
        }
    }

}
