using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xgca.core.Helpers
{
    public class JsonHelper
    {
        public static dynamic SanitizeString(dynamic value)
        {
            return (value.StartsWith("{") && value.EndsWith("}")) ? JObject.Parse(value.Replace("\\", ""))  : value;
        }

        public static dynamic SerializeString(dynamic value)
        {
            return (value.StartsWith("{") && value.EndsWith("}")) ? JsonConvert.SerializeObject(value) : value;
        }
    }
}
