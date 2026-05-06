using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class UserService : ApiService
    {
        public UserService() : base() { }

        public async Task<List<UserProfile>?> GetAllUsersAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<UserProfile>>("/api/Users");
        }

        public async Task<UserProfile?> GetUserByIdAsync(int userId)
        {
            AttachToken();
            return await _http.GetFromJsonAsync<UserProfile>($"/api/Users/{userId}");
        }

        public async Task<User?> CreateUserAsync(CreateUserDto createUserDto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("/api/Users", createUserDto);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<bool> UpdateUserAsync(int userId,UpdateUserDto updateUser)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"/api/Users/{userId}", updateUser);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"/api/Users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}
