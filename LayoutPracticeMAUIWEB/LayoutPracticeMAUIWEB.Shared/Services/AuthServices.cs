using LayoutPracticeMAUIWEB.Shared.Models;
using System.Net.Http.Json;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public class AuthServices
    {
        private readonly HttpClient httpClient;
        public AuthServices(HttpClient _httpClient)
        {
            httpClient = _httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:7202/api/");
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDTO user,string role)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"Auth/register?role={role}", user);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse
                    {
                        Status = "Error",
                        Message = $"API Error: {response.StatusCode}",
                        //Errors = new[] { errorContent }
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "Network error: " + ex.Message
                };
            }
        }
    }
}
