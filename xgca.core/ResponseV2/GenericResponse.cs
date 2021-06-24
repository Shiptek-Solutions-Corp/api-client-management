using System.Collections.Generic;
using System.Linq;

namespace otm.core.ResponseV2
{
    public class GenericResponse<T>
    {
        public GenericResponse(T data)
        {
            Data = data;
        }

        public GenericResponse(T data, List<ErrorField> errors, int statusCode)
        {
            Data = data;
            Errors = errors;
        }

        public GenericResponse(T data, string message)
        {

            Data = data;
            Message = message;
        }

        public GenericResponse(T data, string message, int statusCode)
        {

            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public GenericResponse(T data, List<string> messages, int statusCode)
        {

            Data = data;
            Messages = messages;
            StatusCode = statusCode;
        }

        public GenericResponse(T data, List<ErrorField> errors, string message)
        {
            Data = data;
            Errors = errors;
            Message = message;
        }

        public GenericResponse(T data, List<ErrorField> errors, string message, int statusCode)
        {
            Data = data;
            Errors = errors;
            Message = message;
            StatusCode = statusCode;
        }

        public bool Success => !this.Errors.Any();

        public bool HasErrors => this.Errors.Any();

        public string Message { get; private set; }
        public List<string> Messages { get; private set; }
        public List<ErrorField> Errors { get; private set; } = new List<ErrorField>();
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
    }
}