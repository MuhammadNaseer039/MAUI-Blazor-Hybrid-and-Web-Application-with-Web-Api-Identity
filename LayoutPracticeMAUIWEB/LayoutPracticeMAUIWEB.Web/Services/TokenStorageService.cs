using LayoutPracticeMAUIWEB.Shared.Services;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

namespace LayoutPracticeMAUIWEB.Web.Services
{
    public class TokenStorageService : ITokenStorageService
    {
        private readonly IJSRuntime js;
        public TokenStorageService(IJSRuntime js)
        {
            this.js = js;
        }
        public async Task<string> GetTokenAsync()
        {
            return await js.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        }

        public async Task<DateTime?> GetTokenExpiryAsync()
        {
            var expiryStr = await js.InvokeAsync<string>("localStorage.getItem", "tokenExpiry");
            if (DateTime.TryParse(expiryStr, out var expiry))
                return expiry;
            return null;
        }

        public async Task RemoveTokenAsync()
        {
            await js.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
            await js.InvokeVoidAsync("localStorage.removeItem", "tokenExpiry");
        }

        public async Task SetTokenAsync(string token, DateTime expiry)
        {
            await js.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
            await js.InvokeVoidAsync("localStorage.setItem", "tokenExpiry", expiry);
        }
    }
}