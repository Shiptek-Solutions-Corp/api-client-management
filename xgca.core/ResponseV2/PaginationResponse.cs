using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.ResponseV2
{
    // Custom created for global CMS pagination requested by FE Developers
    public interface IPaginationResponse
    {
        PaginationResponseModel Paginate(dynamic data, long recordCount, int pageNumber, int rowPerPage);
    }

    public class PaginationResponse : IPaginationResponse
    {
        public PaginationResponseModel Paginate(dynamic data, long recordCount, int pageNumber, int rowPerPage)
        {

            if (data is null)
            {
                return new PaginationResponseModel
                {
                    TotalRecords = recordCount,
                    PageNumber = pageNumber,
                    RowPerPage = rowPerPage,
                    PagedResponse = null
                };
            }
            return new PaginationResponseModel
            {
                TotalRecords = recordCount,
                PageNumber = pageNumber,
                RowPerPage = rowPerPage,
                PagedResponse = data
            };
        }
    }
    public class PaginationResponseModel
    {
        public dynamic PagedResponse { get; set; }
        public int PageNumber { get; set; }
        public int RowPerPage { get; set; }
        public long TotalRecords { get; set; }
    }
}
