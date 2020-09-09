using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace xgca.core.Helpers
{
    public static class Constant
    {
        public static string loggedInUserName;

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
        public static string GetValueFromProperty(dynamic data, string key)
        {
            return data.GetType().GetProperty(key).GetValue(data, null);
        }

        public static string[] SplitByComma(string values)
        {
            if (values != null)
            {

                return values.Split(",");
            }
            return new string[] { };
        }
        public static Guid CheckIfGuid(string guidString)
        {
            var isValidGuid = Guid.TryParse(guidString, out var guid);

            if (!isValidGuid) 
                return new Guid();
            else 
                return guid;
        } 
    }
    public interface IParentConstant
    {
        public string BaseUrl { get; set; }
    }
    public abstract class ParentConstant : IParentConstant
    {
        public string BaseUrl { get; set; }
    }
    public class GlobalCmsService : ParentConstant
    {
        
        public string GetServiceDetails { get; set; }
        public string GetService { get; set; }
        public string GetCountry { get; set; }
        public string GetState { get; set; }
        public string GetCity { get; set; }
        public string GetUserType { get; set; }
        public string GetResourcesForAuthorization { get; set; }
    }
    public class OptimusAuthService : ParentConstant
    {
        // Client Menu
        public string EnableUserBatch { get; set; }
        public string DisableUserBatch { get; set; }
        public string EnableUser { get; set; }
        public string DisableUser{ get; set; }
        public string SingleRegisterUser { get; set; }

    }

    public class EmailApi : ParentConstant
    {

    }

    public class EmailTemplate
    {
        public string BaseTemplate { get; set; }
        public string SendContactInviteTemplate { get; set; }
        public string SendProviderInviteTemplate { get; set; }
    }

    public class WebsiteLinks
    {
        public string Login { get; set; }
    }
}
