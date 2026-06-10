using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class AuthService : ApiService
    {
        // You must include a constructor that calls the base constructor
        public AuthService() : base() { }

        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            // _http is now accessible because it is 'protected' in the parent
            var response = await _http.PostAsJsonAsync("/api/Authentication/login",
                new LoginRequest { LoginEmail = email, LoginPassword = password });

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<LoginResponse>()
                : null;
        }

        public async Task<UserProfile?> GetProfileAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<UserProfile>("/api/Users/me");
        }
    }
}
