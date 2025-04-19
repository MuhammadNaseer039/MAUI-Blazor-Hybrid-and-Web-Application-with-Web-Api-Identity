using LayoutPracticeMAUIWEB.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Services
{
    public class HybridTokenStorageService : ITokenStorageService
    {

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync("jwtToken");
        }

        public async Task<DateTime?> GetTokenExpiryAsync()
        {
            var expiryStr = await SecureStorage.GetAsync("tokenExpiry");
            if (DateTime.TryParse(expiryStr, out var expiry))
                return expiry;
            return null;
        }

        public async Task RemoveTokenAsync()
        {
            SecureStorage.Remove("jwtToken");
            SecureStorage.Remove("tokenExpiry");
        }

        public async Task SetTokenAsync(string token, DateTime expiry)
        {
            await SecureStorage.SetAsync("jwtToken", token);
            await SecureStorage.SetAsync("tokenExpiry", expiry.ToString("o"));
        }
    }
}
