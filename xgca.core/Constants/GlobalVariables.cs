using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Constants
{
    public static class GlobalVariables
    {
        public static int SystemUserId = 0;
        public static string SystemUser = "System";

        public static string AuditLogTimeFormat = "yyyy-MM-dd HH:mm";

        public static int ActiveServices = 1;

        public static int Active = 1;
        public static int Inactive = 0;
        public static int Locked = 1;
        public static int Unlocked = 0;

        public static int GuestShipper = 0;
        public static int GuestConsignee = 1;
        public static int GuestNotifyParty = 2;

        public static int LoggedInUserId = 0;
        public static string LoggedInUsername = "system";
        public static int LoggedInCompanyId = 0;

        public static string LoggedInServiceRole = "";
        public static string LoggedInToken = "scheme token";

        public static string AuthToken = "token";
        public static string AuthScheme = "scheme";

        public static List<int> BookingPartyGroup = new List<int>
        {
            (int)Enums.ServiceRole.ShipperConsignee, 
            (int)Enums.ServiceRole.CustomsBroker,
            (int)Enums.ServiceRole.FreightForwarder
        };
    }
}
