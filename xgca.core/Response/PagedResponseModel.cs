using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Response
{
    public class PagedResponseModel
    {
        public dynamic pagedResponse { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
    }
}
