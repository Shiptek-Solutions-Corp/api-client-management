using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.RestApi
{
    public class ApiResponse
    {
        public bool IsSuccessful { get; set; }
        public int StatusCode { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
    }
}
