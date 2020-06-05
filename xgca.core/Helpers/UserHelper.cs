using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;

namespace xgca.core.Helpers
{
    public class UserHelper
    {
        public static dynamic BuildUserValue(dynamic user)
        {
            var obj = new
            {
                user.UserId,
                user.FirstName,
                user.LastName,
                user.MiddleName,
                user.ImageURL,
                user.EmailAddress,
                user.ContactDetails.Landline,
                user.ContactDetails.Mobile,
                user.ContactDetails.Fax,
            };
            return obj;
        }
    }
}
