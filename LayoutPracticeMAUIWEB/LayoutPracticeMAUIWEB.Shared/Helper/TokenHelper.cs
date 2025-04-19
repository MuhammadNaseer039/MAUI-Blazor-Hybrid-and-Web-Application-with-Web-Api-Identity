using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Helper
{
    public static class TokenHelper
    {
        public static bool isTokenExpired(string expiry)
        {
            if(DateTime.TryParse(expiry, out var expiryDate))
            {
                return DateTime.UtcNow > expiryDate;
            }
            return true;
        }
    }
}
