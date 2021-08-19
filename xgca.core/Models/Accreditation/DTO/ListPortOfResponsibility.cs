using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class ListPortOfResponsibility
    {
        public string CompanyId { get; set; }
        public string PortOfLoadingId { get; set; }
        public string PortOfDischargeId { get; set; }
    }

    public class ReturnPortOfResponsibility
    {
        public string PortOfLoadingCompanyId { get; set; }
        public string PortOfDischargeCompanyId { get; set; }
    }
}
