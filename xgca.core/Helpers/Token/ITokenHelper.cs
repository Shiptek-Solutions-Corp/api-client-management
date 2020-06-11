using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.core.Helpers.Token
{
    public interface ITokenHelper
    {
        string RemoveBearer(string encodedToken);
        dynamic DecodeJWT(string encodedToken);
    }
}
