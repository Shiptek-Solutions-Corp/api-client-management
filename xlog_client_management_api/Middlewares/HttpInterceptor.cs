using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xlog_client_management_api.Middlewares
{
    public class HttpInterceptor
    {
        private readonly RequestDelegate _next;

        public HttpInterceptor(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // var hasToken = context.Request.Headers.TryGetValue("Authorization", out var authorization);
                //
                // if (!hasToken)
                // {
                //     context.Response.Clear();
                //     context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //     context.Session.Clear();
                //     await context.Response.WriteAsync("Unauthorized");
                // }
                //
                // var auth = authorization.ToString().Split(" ");
                // var scheme = auth[0];
                // var token = auth[1];
                //
                // context.Session.SetString("scheme", scheme);
                // context.Session.SetString("token", token);

                // var profileId = context.Request.RouteValues["profileId"];
                // context.Session.SetString("profileId", profileId.ToString());

                await _next(context);
            }
            catch (Exception ex)
            {
                // context.Session.Clear();
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
