using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Helpers.Http;
using System.Net.Http.Headers;

namespace xgca.core.Helpers.Http
{
    public class HttpHelper : IHttpHelper
    {
        private static HttpClient _httpClient = new HttpClient();
        public HttpHelper()
        {
        }

        public async Task<dynamic> Get(string environment, string endpointUrl, string key)
        {
            string apiUrl = environment + endpointUrl + key;
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }

        public async Task<dynamic> Get(string environment, string endpointUrl, string key, string token)
        {
            string apiUrl = environment + endpointUrl + key;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }

        public async Task<dynamic> Get(string environment, string endpointUrl)
        {
            string apiUrl = environment + endpointUrl;
            var response = await _httpClient.GetAsync(apiUrl);            
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
        public async Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid, string token)
        {
            string apiUrl = environment + endpointUrl + guid + "/id";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
        public async Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid)
        {
            string apiUrl = environment + endpointUrl + guid + "/id";
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
        public async Task<dynamic> GetGuidById(string environment, string endpointUrl, int id, string token)
        {
            string apiUrl = environment + endpointUrl + id + "/guid";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
        public async Task<dynamic> GetGuidById(string environment, string endpointUrl, int id)
        {
            string apiUrl = environment + endpointUrl + id + "/guid";
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
        public Task<dynamic> Post(string environment, string endpointUrl, dynamic data)
        {

            throw new NotImplementedException();
        }
    }
}
