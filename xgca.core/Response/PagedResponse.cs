using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public interface IPagedResponse
    {
        PagedResponseModel Paginate(dynamic data, int recordCount, int pageNumber, int pageSize);
    }
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
    public class PagedResponseModel
    {
        public dynamic pagedResponse { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
    }
}
