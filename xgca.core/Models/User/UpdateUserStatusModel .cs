namespace xgca.core.Models.User
{
    public class UpdateUserStatusModel
    {
        public string UserId { get; set; }
        public byte Status { get; set; }
        public string ModifiedBy { get; set; }
    }
}
