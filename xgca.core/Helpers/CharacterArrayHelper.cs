using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers
{
    public class CharacterArrayHelper
    {
        public static char[] HtmlEncodeCharacters()
        {
            return new char[] { '<', '/', '>' };
        }

        public static string[] HtmlDecodeCharacters()
        {
            return new string[] { "&quot" };
        }
    }
}
