using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.core.Helpers.QueryFilter
{
    public interface IQueryFilterHelper
    {
        dynamic ParseQueryParams(string query, string delimeter = ",", string queryOperator = "=");
        List<KeyValuePair<string, string>> ParseFilter(String filters);
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
