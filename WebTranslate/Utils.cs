using System;
using System.Collections.Generic;
using System.Text;

namespace WebTranslate
{
    public static class Utils
    {
        public static string UnicodeEncode(string text)
        {
            StringBuilder s = new();
            for (int i = 0; i < text.Length; i++)
            {
                s.Append($"\\u{(int)text[i]:x4}");
            }
            return s.ToString();
        }

    }
}
