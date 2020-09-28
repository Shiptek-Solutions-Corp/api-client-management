using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.Service;

namespace xgca.core.Helpers.Service
{
    public interface IServiceHelper
    {
        List<ListServiceModel> ParseServiceResponse(dynamic serviceObj);
    }
}
