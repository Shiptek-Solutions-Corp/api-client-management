using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public class PagedResponse : IPagedResponse
    {
        public PagedResponseModel Paginate(dynamic d, int recordCount, int pageNumber, int pageSize)
        {
            if (d is null)
            {
                return new PagedResponseModel
                {
                    totalRecords = recordCount,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    pagedResponse = null
                };
            }
            return new PagedResponseModel
            {
                totalRecords = recordCount,
                pageNumber = pageNumber,
                pageSize = pageSize,
                pagedResponse = d
            };
        }
    }
}
