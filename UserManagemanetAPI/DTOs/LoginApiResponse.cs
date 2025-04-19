using UserManagementAPI.DTOs;

namespace UserManagemanetAPI.DTOs
{
    public class LoginApiResponse
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
