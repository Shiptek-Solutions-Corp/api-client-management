using Amazon.S3.Model;

namespace xgca.core.Models.CompanyService
{
    public class ListCompanyServiceModel
    {
        public string CompanyServiceId { get; set; }
        public string ServiceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public byte StaticId { get; set; }
        public byte Status { get; set; }
    }
}
