using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class HealthService : ApiService
    {
        public HealthService() : base() { }

        public async Task<List<HealthSummaryDto>?> GetFilteredHealthRecordAsync(int? childId, int? centerId)
        {
            AttachToken();
            var url = $"/api/Health?childId={childId}&centerId={centerId}";

            try
            {
                return await _http.GetFromJsonAsync<List<HealthSummaryDto>>(url);
            }
            catch (Exception)
            {
                return new List<HealthSummaryDto>();
            }
        }

        public async Task<HealthSummaryDto> CreateHealthRecordAsync(CreateHealthDto newHealthRecord, int cdwWorkerId)
        {
            AttachToken();

            var response = await _http.PostAsJsonAsync("/api/Health", newHealthRecord);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<HealthSummaryDto>();
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {errorBody}");

            return null;
        }

        public async Task<bool> UpdateHealthRecordAsync(UpdateHealthDto updatedHealthRecord, int cdwWorkerId)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"/api/Health/{cdwWorkerId}", updatedHealthRecord);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteHealthRecordAsync(int healthRecordId)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"/api/Health/{healthRecordId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AbnormalBmiDto>?> GetAbnormalBmiDtoAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<AbnormalBmiDto>>("/api/Health/abnormal-bmi");
        }
    }
}
