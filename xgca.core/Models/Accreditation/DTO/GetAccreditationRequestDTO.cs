using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request.DTO
{
    public class GetAccreditationRequestDTO
    {
        public GetAccreditationRequestDTO()
        {
            search = "";
            pageNumber = 0;
            rowPerPage = 10;
            columnSort = "companyName";
            sort = "ASC";
            CcompanyName = "";
            CfullAddress = "";
            Cstatus = "";
            Coperating = "";
            CportResp = "";
            ExportTo = "";
            CLocode = "";
        }
        public int companyId { get; set; }
        public string search { get; set; }
        public int pageNumber { get; set; }
        public int rowPerPage { get; set; }
        public string columnSort { get; set; }
        public string sort { get; set; }
        public string CcompanyName { get; set; }
        public string CfullAddress { get; set; }
        public string Cstatus { get; set; }
        public string Coperating { get; set; }
        public string CportResp { get; set; }
        public string CLocode { get; set; }
        public string ExportTo { get; set; }
    }
}
