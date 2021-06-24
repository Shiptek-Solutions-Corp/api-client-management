using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Company
{
    public class CreateCompanyViewModel
    {

    }
    public class GetCompanyListingViewModel
    {
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string ServiceName { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
        public string KycStatusCode { get; set; }
        public string Guid { get; set; }
    }

    public class UpdateCompanyViewModel : CreateCompanyViewModel
    {

    }
}
