using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Helpers.Http;
using System.Net.Http.Headers;
using xgca.core.Constants;
using xgca.core.Models.Email;
using Microsoft.Extensions.Options;

namespace xgca.core.Helpers.Http
{
    public interface IHttpHelper
    {
        public Task<dynamic> Post(string endpointUrl, dynamic data, string token);

        public Task<dynamic> CustomGet(string environment, string endpointUrl, string token);
        public Task<dynamic> CustomGet(string environment, string endpointUrl);

        public Task<dynamic> Get(string environment, string endpointUrl, string key);
        public Task<dynamic> Get(string environment, string endpointUrl, string key, string token);
        public Task<dynamic> Get(string environment, string endpointUrl);
        public Task<dynamic> PostGetResource(string environment, string endpointUrl, dynamic data);

        public Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid);
        public Task<dynamic> GetIdByGuid(string environment, string endpointUrl, string guid, string token);
        public Task<dynamic> GetGuidById(string environment, string endpointUrl, int id);
        public Task<dynamic> GetGuidById(string environment, string endpointUrl, int id, string token);
        public Task<dynamic> Put(string endpointUrl, dynamic data, string token);

    }
    public class HttpHelper : IHttpHelper
    {
        private HttpClient _httpClient;
        private readonly IOptions<EmailApi> emailOptions;

        public HttpHelper(IOptions<EmailApi> emailOptions)
        {
            _httpClient =  new HttpClient();
            this.emailOptions = emailOptions;
        }

        public async Task<dynamic> CustomGet(string environment, string endpointUrl, string token)
        {
            string apiUrl = environment + endpointUrl;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }

        public async Task<dynamic> CustomGet(string environment, string endpointUrl)
        {
            string apiUrl = environment + endpointUrl;
            var response = await _httpClient.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
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
        public async Task<dynamic> Post(string endpointUrl, dynamic data, string token)
        {
            if (!(token is null))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (data is EmailPayload)
            {
                _httpClient.DefaultRequestHeaders.Add("x-api-key", emailOptions.Value.ApiKey);
            }

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpointUrl, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return new
                {
                    Message = "Error encountered on sending email",
                    StatusCode = (int)response.StatusCode,
                    Status = false
                };
            }

            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }

        public async Task<dynamic> Put(string endpointUrl, dynamic data, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            var httpContent = new StringContent(json,
                                    Encoding.UTF8,
                                    "application/json");

            // PostAsync returns a Task<httpresponsemessage>
            var httpResponce = await _httpClient.PutAsync(endpointUrl, httpContent);

            var result = await httpResponce.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }

        public async Task<dynamic> PostGetResource(string environment, string endpointUrl, dynamic data)
        {
            string apiUrl = environment + endpointUrl;

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var httpContent = new StringContent(json,
                                    Encoding.UTF8,
                                    "application/json");

            var response = await _httpClient.PostAsync(apiUrl, httpContent);

            var result = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject(result);

            return responseData;
        }
    }
}
