using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Helpers.Http;
using xgca.core.Response;

namespace xlog_client_management_api
{
    public class TokenAuthorizeAttribute : TypeFilterAttribute
    {
        public TokenAuthorizeAttribute(string claimType, string claimValue) : base(typeof(TokenAuthorizeFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class TokenAuthorizeFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        private readonly IHttpHelper _helpers;

        public TokenAuthorizeFilter(Claim claim, IHttpHelper helpers)
        {
            _claim = claim;
            _helpers = helpers;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isMasterUser = context.HttpContext.User.Claims.Any(t => t.Type == "custom:mUser" && t.Value.Contains("1"));

            if (!isMasterUser)
            {
                var hasPermission = context.HttpContext.User.Claims.Any(t => t.Type == _claim.Type && t.Value.Contains(_claim.Value));
                if (!hasPermission)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = new ForbidResult();
                    GeneralModel general = new GeneralModel { isSuccessful = false, data = null, statusCode = 403, message = "You have insufficient permission to access this module" };
                    byte[] data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(general));

                    context.HttpContext.Response.ContentLength = Newtonsoft.Json.JsonConvert.SerializeObject(general).Length;

                    context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
                }
            }
        }
    }
}
