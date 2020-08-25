using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public interface IPagedResponse
    {
        PagedResponseModel Paginate(dynamic data, int recordCount, int pageNumber, int pageSize);
    }
}
