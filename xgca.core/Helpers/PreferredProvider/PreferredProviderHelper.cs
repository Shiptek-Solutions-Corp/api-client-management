using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xgca.core.Models.PreferredProvider;

namespace xgca.core.Helpers.PreferredProvider
{
    public class PreferredProviderHelper : IPreferredProviderHelper
    {
        public List<ListPreferredProvider> SortProviders(List<ListPreferredProvider> providers, string sortBy, string sortOrder)
        {
            List<ListPreferredProvider> temp = providers;

            if (sortBy is null || sortOrder is null)
                return temp.ToList();

            if (sortOrder.Equals("asc"))
            {
                if (sortBy.ToLower().Equals("companyname")) temp = (List<ListPreferredProvider>)providers.OrderBy(x => x.CompanyName).ToList();
                if (sortBy.ToLower().Equals("state")) temp = (List<ListPreferredProvider>)providers.OrderBy(x => x.State).ToList();
                if (sortBy.ToLower().Equals("country")) temp = (List<ListPreferredProvider>)providers.OrderBy(x => x.Country).ToList();
                if (sortBy.ToLower().Equals("service")) temp = (List<ListPreferredProvider>)providers.OrderBy(x => x.ServiceName).ToList();
            }
            else
            {
                if (sortBy.ToLower().Equals("companyname")) temp = (List<ListPreferredProvider>)providers.OrderByDescending(x => x.CompanyName).ToList();
                if (sortBy.ToLower().Equals("state")) temp = (List<ListPreferredProvider>)providers.OrderByDescending(x => x.State).ToList();
                if (sortBy.ToLower().Equals("country")) temp = (List<ListPreferredProvider>)providers.OrderByDescending(x => x.Country).ToList();
                if (sortBy.ToLower().Equals("service")) temp = (List<ListPreferredProvider>)providers.OrderByDescending(x => x.ServiceName).ToList();
            }

            return temp;
        }
    }
}
