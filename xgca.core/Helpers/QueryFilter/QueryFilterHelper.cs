using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.Service;

namespace xgca.core.Helpers.QueryFilter
{
    public interface IQueryFilterHelper
    {
        dynamic ParseQueryParams(string query, string delimeter = ",", string queryOperator = "=");
        List<KeyValuePair<string, string>> ParseFilter(String filters);
        List<int> CheckForFilterForService(List<KeyValuePair<string, string>> filterList, List<ListServiceModel> services);
    }
    public class QueryFilterHelper : IQueryFilterHelper
    {
        public QueryFilterHelper() { }

        public List<KeyValuePair<string, string>> ParseFilter(String filter)
        {
            var list = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(filter))
            {
                string[] words = filter.Split(',');
                foreach (string word in words)
                {
                    string[] val = word.Split(':');
                    list.Add(new KeyValuePair<string, string>(val[0], val[1]));
                }
            }

            return list;
        }

        public List<int> CheckForFilterForService(List<KeyValuePair<string, string>> filterList, List<ListServiceModel> services)
        {
            List<int> serviceIds = new List<int>();

            foreach (var filter in filterList)
            {
                if (filter.Key.ToLower().Equals("service"))
                {
                    var serviceFilters = services.Where(x => x.ServiceName.Contains(filter.Value) || x.ServiceName.StartsWith(filter.Value) || x.ServiceName.EndsWith(filter.Value)).ToList();
                    if (serviceFilters.Count == 0)
                    {
                        serviceIds.Add(0);
                    }
                    else
                    {
                        foreach(var service in serviceFilters)
                        {
                            serviceIds.Add(service.IntServiceId);
                        }
                    }
                }
            }

            return serviceIds;
        }

        public dynamic ParseQueryParams(string query, string delimeter = ",", string queryOperator = "=")
        {
            var queryParams = query.Split(delimeter)
                                    .Select(p => p.Split(':'))
                                    .ToDictionary(p => p[0], p => p.Length > 1 ? Uri.EscapeDataString(p[1]) : null);

            var queryString = "";

            foreach (var v in queryParams)
            {
                string conjunction = " and ";
                string enclosure = "";
                if (queryOperator.Equals("LIKE"))
                {
                    conjunction = "";
                    enclosure = "%";
                }
                queryString += $"{v.Key} {queryOperator} '{enclosure}{v.Value}{enclosure}'{conjunction}";
            }

            return queryString;
        }

        
    }
}
