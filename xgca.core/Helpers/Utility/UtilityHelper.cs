using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xgca.core.Helpers.Utility
{
    public class UtilityHelper : IUtilityHelper
    {
        public string GeteRandomStrings(int len)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
