using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class GetCompanyListingDTO
    {
        public GetCompanyListingDTO()
        {
            QuickSearch = "";
            pageNumber = 0;
            rowPerPage = 10;
            columnSort = "companyName";
            sort = "ASC";
            CcompanyName = "";
            CcountryName = "";
            Cstatus = "";
            ExportTo = "";
            Ccity = "";
            Cstate = "";
        }
        public int companyId { get; set; }
        public string QuickSearch { get; set; }
        public int pageNumber { get; set; }
        public int rowPerPage { get; set; }
        public string columnSort { get; set; }
        public string sort { get; set; }
        public string CcompanyName { get; set; }
        public string CcountryName { get; set; }
        public string Ccity { get; set; }
        public string Cstate { get; set; }
        public string Cstatus { get; set; }
        public string ExportTo { get; set; }
    }

}
