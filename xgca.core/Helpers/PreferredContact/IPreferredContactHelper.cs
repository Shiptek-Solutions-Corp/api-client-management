using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers.Utility
{
    public interface IPreferredContactHelper
    {
        string GuestAddress(dynamic obj);
        string RegisteredAddress(dynamic obj);
        string GuestCityState(dynamic obj);
        string RegisteredCityState(dynamic obj);
        entity.Models.PreferredContact ParseObject(string guestId, string companyId, int profileId, int contactType, int createdBy);
        dynamic BuildCompanyDetails(string preferredContactGuid, dynamic company, dynamic state, dynamic city, dynamic masterUser);
        dynamic BuildGuestDetails(string preferredContactGuid, dynamic guest);
    }
}
