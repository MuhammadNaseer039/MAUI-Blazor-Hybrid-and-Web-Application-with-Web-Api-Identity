using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public interface ITokenStorageService
    {
        Task SetTokenAsync(string token, DateTime expiry);
        Task<string> GetTokenAsync();
        Task<DateTime?> GetTokenExpiryAsync();
        Task RemoveTokenAsync();
    }
}
