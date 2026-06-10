using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class FeeService : ApiService
    {
        public FeeService() : base() { }

        public async Task<List<FeeSummaryDto>?> GetFilteredFeeRecordsAsync(int? childId, int? centerId, string? receiptNo)
        {
            AttachToken();
            var url = $"/api/Fees?childId={childId}&centerId={centerId}&receiptNo={receiptNo}";
            try
            {
                return await _http.GetFromJsonAsync<List<FeeSummaryDto>>(url);
            }
            catch (Exception)
            {
                return new List<FeeSummaryDto>();
            }
        }

        public async Task<FeeCalculatedDto> GetFeeCalculationsAsync(int childId)
        {
            try
            {
                // Must match the [HttpGet("calculated/{childId}")] in your controller
                var response = await _http.GetAsync($"api/fees/calculated/{childId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    // This is the most common reason for $0.00: 
                    // Without this, System.Text.Json won't map "feeTotalAmountPaid" to "FeeTotalAmountPaid"
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<FeeCalculatedDto>(content, options);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"API Error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> MarkFeeAsPaidAsync(int feeId)
        {
            AttachToken();
            var response = await _http.PutAsync($"/api/Fees/{feeId}/pay", null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {errorBody}");
            return false;
        }

        public async Task<FeeSummaryDto> CreateFeeRecordAsync(CreateFeeDto newFeeRecord, int cdwWorkerId)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("/api/Fees", newFeeRecord);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FeeSummaryDto>();
            }
            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {errorBody}");
            return null;
        }

        // Services/FeeService.cs

        public async Task<FeesSummaryDto?> GetOverallFeesSummaryAsync(int? centerId, int? month, int? year)
        {
            AttachToken();
            try
            {
                // Dynamically build the query string based on provided parameters
                var queryParams = new List<string>();
                if (centerId.HasValue) queryParams.Add($"centerId={centerId.Value}");
                if (month.HasValue) queryParams.Add($"month={month.Value}");
                if (year.HasValue) queryParams.Add($"year={year.Value}");

                string queryString = queryParams.Count > 0
                    ? "?" + string.Join("&", queryParams)
                    : string.Empty;

                // GET /api/fees/summary with its optional query filters
                var response = await _http.GetAsync($"/api/Fees/summary{queryString}");

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<FeesSummaryDto>(content, options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[FeeService] GetOverallFeesSummary error: {ex.Message}");
                return null;
            }
        }

        public async Task<FeeSummaryDto?> UpdateFeeRecordAsync(int feeId, UpdateFeeDto updatedFeeRecord)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"/api/Fees/{feeId}", updatedFeeRecord);

            if (response.IsSuccessStatusCode)
            {
                // Check if there is actually content to read
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.Content.Headers.ContentLength == 0)
                {
                    // If the server returns No Content, return a dummy or null 
                    // depending on how your UI handles the result.
                    return new FeeSummaryDto { FeeId = feeId };
                }

                try
                {
                    return await response.Content.ReadFromJsonAsync<FeeSummaryDto>();
                }
                catch (JsonException)
                {
                    // Fallback if JSON is malformed but status was success
                    return new FeeSummaryDto { FeeId = feeId };
                }
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {errorBody}");
            return null;
        }

        public async Task<bool> DeleteFeeRecordAsync(int feeId)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"/api/Fees/{feeId}");
            if (response.IsSuccessStatusCode)
            {
                // Converter removed per earlier instruction layout preferences.
                return true;
            }
            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {errorBody}");
            return false;
        }
    }
}