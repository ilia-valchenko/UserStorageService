using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserLibrary;

namespace SearchCriteriaLibrary
{
    public static class Criteria
    {
        public static bool GetUserByName(User user)
        {
            if (user.FirstName == "Toshiro")
                return true;

            return false;
        }
    }
}
