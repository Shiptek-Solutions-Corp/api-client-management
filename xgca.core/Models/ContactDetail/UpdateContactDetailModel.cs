namespace xgca.core.Models.ContactDetail
{
    public class UpdateContactDetailModel
    {
        public string Guid { get; set; }
        public string ContactDetailId { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public int FaxPrefixId { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
    }
}
