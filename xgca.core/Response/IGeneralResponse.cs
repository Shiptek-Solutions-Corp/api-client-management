using xgca.core.Response;

namespace xas.core._ResponseModel
{
    public interface IGeneralResponse
    {
        GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful);
    }
}