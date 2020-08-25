using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers.Guest
{
    public interface IGuestHelper
    {
        string ParseCompleteAddress(entity.Models.Guest guest);
    }
}
