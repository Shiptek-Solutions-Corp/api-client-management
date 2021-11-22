using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Constants;

namespace xas.core._Helpers
{
    public static class AuthenticationHelper
    {
        public static (string, string) GetToken()
        {
            string[] token = GlobalVariables.LoggedInToken.Split(" ");
            return (token[0], token[1]);
        }
    }
}
