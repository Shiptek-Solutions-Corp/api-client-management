namespace xgca.core.Response
{
    public interface IGeneral
    {
        GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful);
    }
}