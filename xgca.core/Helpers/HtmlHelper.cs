using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace xgca.core.Helpers
{
    public class HtmlHelper
    {
        public static string EncodeHTMLValue(string htmlValue)
        {
            return HttpUtility.HtmlEncode(htmlValue);
        }

        public static string DecodeHTMLValue(string htmlValue)
        {
            return HttpUtility.HtmlDecode(htmlValue);
        }

        public static bool CheckForScriptTag(string content)
        {
            if (content.ToLower().Contains("html") || content.Contains("<script>") || content.Contains("</script>"))
            {
                return true;
            }
            return false;
        }

        public static string StripScriptTag(string htmlValue)
        {
            string stripped = "";
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            stripped = rRemScript.Replace(htmlValue, "");
            rRemScript = new Regex(@"< script [ ^ > ] * >[\s\S]*?</ script >");
            return rRemScript.Replace(stripped, "");
        }
    }
}
