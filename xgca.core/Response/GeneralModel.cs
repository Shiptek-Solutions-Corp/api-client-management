using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public interface IGeneralModel
    {
        dynamic data { get; set; }
        bool isSuccessful { get; set; }
        string message { get; set; }
        int statusCode { get; set; }
        List<ErrorField> errors { get; set; }
    }
    public class GeneralModel : IGeneralModel
    {
        public bool isSuccessful { get; set; }
        public int statusCode { get; set; }
        public dynamic data { get; set; }
        public string message { get; set; }
        public List<ErrorField> errors { get; set; }

    }
}
