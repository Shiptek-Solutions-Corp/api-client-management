using Microsoft.Extensions.Configuration.UserSecrets;
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
                user.ContactDetails.Phone,
                user.ContactDetails.Mobile,
                user.ContactDetails.Fax,
            };
            return obj;
        }

        public static dynamic BuildUserLogValue(dynamic user, int userId, int createdBy)
        {
            var obj = new
            {
                UserId = userId,
                user.FirstName,
                user.LastName,
                user.MiddleName,
                user.ImageURL,
                user.EmailAddress,
                user.ContactDetails.Phone,
                user.ContactDetails.Mobile,
                user.ContactDetails.Fax,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
            return obj;
        }
    }
}
