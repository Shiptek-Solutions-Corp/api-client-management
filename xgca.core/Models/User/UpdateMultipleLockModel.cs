using System;
using System.Collections.Generic;

namespace xgca.core.Models.User
{
    public class UpdateMultipleLockModel
    {
        public List<string> UserId { get; set; }
        public byte IsLocked { get; set; }
        public int ModifiedBy { get; set; }
    }
}
