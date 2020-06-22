using System.Collections.Generic;

namespace xgca.core.Response
{
    public interface IGeneral
    {
        GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful);
        GeneralModel Response(dynamic data, List<ErrorField> errors, int statusCode, string message, bool isSuccessful);
    }
}