using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public class GeneralModel : IGeneralModel
    {
        public bool isSuccessful { get; set; }
        public int statusCode { get; set; }
        public dynamic data { get; set; }
        public string message { get; set; }
        public List<ErrorField> errors { get; set; }

    }
}
