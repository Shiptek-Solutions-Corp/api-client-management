using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public interface IGeneral
    {
        GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful);
        GeneralModel Response(dynamic data, List<ErrorField> errors, int statusCode, string message, bool isSuccessful);
    }
    public class General : IGeneral
    {
        public GeneralModel Response(dynamic data, int statusCode, string message, bool isSuccessful)
        {
            var dataResponse = new GeneralModel() { 
                isSuccessful = isSuccessful, 
                statusCode = statusCode, 
                message = message, 
                data = data 
            };
            return dataResponse;
        }

        public GeneralModel Response(dynamic data, List<ErrorField> errors, int statusCode, string message, bool isSuccessful)
        {
            var dataResponse = new GeneralModel()
            {
                isSuccessful = isSuccessful,
                statusCode = statusCode,
                message = message,
                data = data,
                errors = errors
            };
            return dataResponse;
        }
    }
}
