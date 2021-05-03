using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOSApp.Core.Utils
{
    public class TextProcessor
    {
        public static string ToPronounCasing(string word)
        {
            word = word.Trim();
            return word.Substring(0, 1).ToUpper() + word[1..].ToLower();
        }
    }
}
