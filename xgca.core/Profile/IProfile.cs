using System;
using System.Threading.Tasks;

namespace xgca.core.Profile
{
    public interface IProfile
    {
        Task<dynamic> LoadProfile(string username, string companyServiceKey);
    }
}
