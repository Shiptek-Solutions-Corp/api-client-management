using System.Collections.Generic;
using xgca.core.Response;

namespace xas.core._ResponseModel
{
    public interface IGeneralResponse
    {
        GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful);
        GeneralModel Response(dynamic data, List<ErrorField> errors, int statusCode, string message, bool isSuccessful);
    }
}