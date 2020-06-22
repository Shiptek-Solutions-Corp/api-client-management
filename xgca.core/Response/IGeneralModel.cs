using System.Collections.Generic;

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
}