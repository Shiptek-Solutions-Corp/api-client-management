using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.core.Helpers.Http
{
    public interface IHttpHelper
    {
        public Task<dynamic> Post(string environment, string endpointUrl, dynamic data);

        public Task<dynamic> CustomGet(string environment, string endpointUrl, string token);
        public Task<dynamic> CustomGet(string environment, string endpointUrl);

        public Task<dynamic> Get(string environment, string endpointUrl, string key);
        public Task<dynamic> Get(string environment, string endpointUrl, string key, string token);
        public Task<dynamic> Get(string environment, string endpointUrl);
        public Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid);
        public Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid, string token);
        public Task<dynamic> GetGuidById(string environment, string endpointUrl, int id);
        public Task<dynamic> GetGuidById(string environment, string endpointUrl, int id, string token);
        public Task<dynamic> Put(string endpointUrl, dynamic data, string token);
        public Task<dynamic> PutWithoutBody(string endpointUrl, string token);

    }
}
