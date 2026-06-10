using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class AttendanceService : ApiService
    {
        public AttendanceService() : base() { }

        public async Task<List<AttendanceSummaryDto>> GetFilteredAttendanceAsync(int day, int month, int year, int? childId, int? centerId)
        {
            AttachToken();
            string url = $"api/Attendance?day={day}&month={month}&year={year}";

            if (childId.HasValue)
                url += $"&childId={childId.Value}";

            if (centerId.HasValue)
                url += $"&centerId={centerId.Value}";

            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AttendanceSummaryDto>>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
        }

        public async Task<AttendanceSummaryDto> CreateAttendanceAsync(CreateAttendanceDto dto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("api/attendance", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AttendanceSummaryDto>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Create Failed: {response.StatusCode} - {errorContent}");
        }

        public async Task<bool> UpdateAttendanceAsync(int id, UpdateAttendanceDto dto)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"api/attendance/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Update Failed: {response.StatusCode} - {errorContent}");
        }

        public async Task<bool> DeleteAttendanceAsync(int id)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"api/attendance/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Delete Failed: {response.StatusCode} - {errorContent}");
        }
    }
}