using System;
using System.Collections.Generic;

namespace xgca.core.Models.User
{
    public class UpdateMultipleStatusModel
    {
        public List<string> UserId { get; set; }
        public byte Status { get; set; }
        public int ModifiedBy { get; set; }
    }
}
