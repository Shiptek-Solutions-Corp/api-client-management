using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.PreferredProvider;

namespace xgca.core.Helpers.PreferredProvider
{
    public interface IPreferredProviderHelper
    {
        List<ListPreferredProvider> SortProviders(List<ListPreferredProvider> providers, string sortBy, string sortOrder);
    }
}
