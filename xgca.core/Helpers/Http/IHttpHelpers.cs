using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.core.Helpers.Http
{
    public interface IHttpHelpers
    {
        public Task<dynamic> Post(string environment, string endpointUrl, dynamic data);

        public Task<dynamic> Get(string environment, string endpointUrl, string key);
        public Task<dynamic> Get(string environment, string endpointUrl);

        public Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid);
        public Task<dynamic> GetGuidById(string environment, string endpointUrl, int id);
    }
}
