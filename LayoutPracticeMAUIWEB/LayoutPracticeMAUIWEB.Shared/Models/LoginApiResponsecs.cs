using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Models
{
    public class LoginApiResponsecs
    {
        public string Token { get; set; }
        public string Expiry { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
