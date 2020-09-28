using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.Service;

namespace xgca.core.Helpers.Service
{
    public class ServiceHelper : IServiceHelper
    {
        public List<ListServiceModel> ParseServiceResponse(dynamic serviceObj)
        {
            List<ListServiceModel> services = new List<ListServiceModel>();
            foreach (var service in serviceObj)
            {
                services.Add(new ListServiceModel
                {
                    IntServiceId = service.intServiceId,
                    ServiceId = service.serviceId,
                    ServiceCode = service.serviceCode,
                    ServiceName = service.serviceName,
                    ServiceImageURL = service.imageURL
                });
            }

            return services;
        }
    }
}
