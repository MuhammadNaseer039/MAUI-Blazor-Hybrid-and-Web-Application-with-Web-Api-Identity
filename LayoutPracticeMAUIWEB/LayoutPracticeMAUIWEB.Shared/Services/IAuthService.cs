using LayoutPracticeMAUIWEB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public interface IAuthService
    {
        Task<LoginApiResponsecs> LoginAsync(LoginDTO user);
        Task<ApiResponse> RegisterAsync(RegisterDTO user, string role);
    }
}
