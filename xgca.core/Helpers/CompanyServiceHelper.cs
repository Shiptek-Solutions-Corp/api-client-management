using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers
{
    public class CompanyServiceHelper
    {
        public static dynamic BuildCompanyServiceValue(dynamic obj)
        {
            var data = new
            {
                CompanyServiceId = obj.CompanyServiceId,
                ServiceId = obj.ServiceId,
                ServicenName = obj.Services.ServicenName,
                Status = obj.Status
            };
            return data;
        }

        public static dynamic BuildCompanyServiceList(dynamic obj)
        {
            List<dynamic> data = new List<dynamic>();
            foreach(var service in obj)
            {
                data.Add(new
                {
                    service.Code,
                    service.Name,
                    service.ImageURL,
                    service.Status
                });
            }

            return data;
        }
    }
}
