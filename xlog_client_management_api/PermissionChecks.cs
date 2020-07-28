using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public TokenAuthorizeFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var hasPermission = context.HttpContext.User.Claims.Any(t => t.Type == _claim.Type && t.Value.Contains(_claim.Value));
            if (!hasPermission)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Result = new ForbidResult();
            }
        }
    }
}
