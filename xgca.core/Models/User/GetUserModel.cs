using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class GetUserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int ContactDetailId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string EmailAddress { get; set; }
        public string ImageURL { get; set; }
        public Guid Guid { get; set; }
    }
}
