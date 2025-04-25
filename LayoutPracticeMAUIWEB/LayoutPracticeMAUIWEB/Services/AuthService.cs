using LayoutPracticeMAUIWEB.Shared.Models;
using LayoutPracticeMAUIWEB.Shared.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace LayoutPracticeMAUIWEB.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;
        private readonly ITokenStorageService tokenStorage;
        private readonly CustomAuthStateProvider customauth;
        private readonly NavigationManager navigationManager;
        public AuthService(HttpClient http, ITokenStorageService tokenStorage, CustomAuthStateProvider customauth, NavigationManager navigationManager)
        {
            this.http = http;
            http.BaseAddress = new Uri("https://localhost:7202/api/");
            this.tokenStorage = tokenStorage;
            this.customauth = customauth;
            this.navigationManager = navigationManager;
        }
        public async Task<LoginApiResponsecs> LoginAsync(LoginDTO user)
        {
            try
            {
                var response = await http.PostAsJsonAsync("Auth/login", user);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginApiResponsecs>();

                    if (!string.IsNullOrEmpty(result.Token) && DateTime.TryParse(result.Expiry, out var expiry))
                    {
                        await tokenStorage.SetTokenAsync(result.Token,expiry);
                    }
                    return result;
                }
                else
                {
                    var errorcontent = await response.Content.ReadAsStringAsync();
                    return new LoginApiResponsecs
                    {
                        Token = "",
                        Expiry = "",
                        Email = "",
                        Status = "Error",
                        Message = $"API Error: {response.StatusCode} " + errorcontent
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new LoginApiResponsecs
                {
                    Token = "",
                    Expiry = "",
                    Email = "",
                    Status = "Error",
                    Message = "Network Error: " + ex.Message
                };
            }
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDTO user, string role)
        {
            try
            {
                var response = await http.PostAsJsonAsync($"Auth/register?role={role}", user);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse>();
                }
                else
                {
                    var errorcontent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse
                    {
                        Status = "Error",
                        Message = $"API Error: {response.StatusCode} " + errorcontent
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse { Status = "Error", Message = "Network Error" + ex.Message };
            }
        }

        public async Task LogoutAsync()
        {
            await tokenStorage.RemoveTokenAsync();

            customauth.NotifyUserLogout();
            navigationManager.NavigateTo("/login", true);
        }
    }
}
