using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class CenterService : ApiService
    {
        public CenterService() : base() { }

        public async Task<List<Center>?> GetAllCentersAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<Center>>("/api/Centers");
        }

        public async Task<CenterDetailsDto?> GetCenterByIdAsync(int centerId)
        {
            AttachToken();
            return await _http.GetFromJsonAsync<CenterDetailsDto>($"/api/centers/{centerId}");
        }

        public async Task<Center?> CreateCenterAsync(Center center)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("/api/Centers", center);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Center>();
            }
            return null;
        }

        public async Task<bool> UpdateCenterAsync(int id, Center center)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"/api/Centers/{id}", center);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCenter(int id)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"/api/Centers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
