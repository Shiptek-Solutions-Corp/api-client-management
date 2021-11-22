using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core._Helpers.IOptionModels
{
    public class ClientManagement
    {
        public string BasePath { get; set; }
        public string CompanyByServiceId { get; set; }
        public string CompanyIdByGuid { get; set; }
        public string CompanyGuidById { get; set; }
        public string CompanyDetailsByGuids { get; set; }
        public string CheckIfCompanyExists { get; set; }
        public string UpdateAccreditedBy { get; set; }
        public string BulkCheckIfExistsByCompanyName { get; set; }
    }
}
