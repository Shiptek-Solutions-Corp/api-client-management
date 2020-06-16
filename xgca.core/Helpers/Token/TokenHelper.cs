using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace xgca.core.Helpers.Token
{
    public class TokenHelper : ITokenHelper
    {

        public string RemoveBearer(string encodedToken)
        {
            if (encodedToken.Contains("Bearer"))
            {
                string[] tokenArray = encodedToken.Split(" ");
                return tokenArray[1];
            }
            return encodedToken;
            
        }
        public dynamic DecodeJWT(string encodedToken)
        {
            var jwt = this.RemoveBearer(encodedToken);
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(jwt);

            return decodedToken;
        }
    }
}
