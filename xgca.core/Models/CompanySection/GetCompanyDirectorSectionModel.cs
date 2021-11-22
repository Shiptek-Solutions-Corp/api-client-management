using xgca.core.Models.CompanyDirector;
using System.Collections.Generic;

namespace xgca.core.Models.CompanySection
{
    public class GetCompanyDirectorSectionModel
    {
        public string Id { get; set; }
        public string SectionStatusCode { get; set; }
        public string SectionStatusName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public bool IsDraft { get; set; }
        public List<GetCompanyDirectorModel> Directors { get; set; }
        public string LatestRemarks { get; set; }
    }
}
