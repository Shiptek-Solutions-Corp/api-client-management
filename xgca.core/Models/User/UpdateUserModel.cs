namespace xgca.core.Models.User
{
    public class UpdateUserModel
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public string WebsiteURL { get; set; }
        //public string EmailAddress { get; set; }
        public string ContactDetailId { get; set; }
        public string ModifiedBy { get; set; }
    }
}
