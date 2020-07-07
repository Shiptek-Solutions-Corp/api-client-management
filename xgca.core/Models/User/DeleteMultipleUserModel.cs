using System;
using System.Collections.Generic;

namespace xgca.core.Models.User
{
    public class DeleteMultipleUserModel
    {
        public List<string> UserId { get; set; }
        public int ModifiedBy { get; set; }
    }
}
