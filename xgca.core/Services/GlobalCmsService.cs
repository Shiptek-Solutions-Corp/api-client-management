using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Helpers.Http;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.GlobalCms;
using xgca.core.Models.RestApi;

namespace xgca.core.Services
{
    public interface IGLobalCmsService
    {
        Task<List<ServicesModel>> GetAllService();
        Task<JArray> GetAllResourceByGroupResourceIds(int[] ids, string token);
    }
    public class GlobalCmsService : IGLobalCmsService
    {
        private readonly IHttpHelper httpHelper;
        private readonly IOptions<core.Helpers.GlobalCmsService> options;
        private readonly IMapper mapper;
        public GlobalCmsService(
            IHttpHelper httpHelper, 
            IOptions<core.Helpers.GlobalCmsService> options,
            IMapper mapper
            )
        {
            this.httpHelper = httpHelper;
            this.options = options;
            this.mapper = mapper;
        }

        public async Task<JArray> GetAllResourceByGroupResourceIds(int[] ids, string token)
        {
            var response = await httpHelper.GetWithToken(options.Value.BaseUrl, options.Value.GetResourcesForAuthorization, new { ids = ids}, token);
            if (response.data != null)
            {
                var services = response.data?["resources"] as JArray;
                return services;
            }
            return new JArray { };
        }

        public async Task<List<ServicesModel>> GetAllService()
        {
            var response = await httpHelper.Get(options.Value.BaseUrl, options.Value.GetService);
            var services = (response.data["services"] as JArray)?.ToObject<List<ServicesModel>>();
            return services;
        }
    }
}
