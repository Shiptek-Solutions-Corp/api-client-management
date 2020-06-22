namespace xgca.core.Models.User
{
    public class UpdateUserLockModel
    {
        public string UserId { get; set; }
        public byte IsLocked { get; set; }
        public string ModifiedBy { get; set; }
    }
}
