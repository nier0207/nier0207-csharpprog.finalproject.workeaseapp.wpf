using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class ReportService : ApiService
    {
        public ReportService() : base() { }

        // ── MASTER LIST ──────────────────────────────────────────────────────────
        public async Task<ReportListDto> GenerateMasterListAsync(GenerateMasterListDto dto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("api/reports/master-list", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReportListDto>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Generation Failed: {error}");
        }

        // ── PDF SUMMARY ──────────────────────────────────────────────────────────
        public async Task<ReportListDto> GeneratePdfSummaryAsync(GeneratePdfSummaryDto dto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("api/reports/pdf-summary", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReportListDto>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"PDF Generation Failed: {error}");
        }

        // ── FEE REPORT ───────────────────────────────────────────────────────────
        public async Task<ReportListDto> GenerateReportFeeAsync(GenerateReportFeeDto dto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("api/reports/fee-report", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReportListDto>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Fee Report Generation Failed: {error}");
        }

        // ── NARRATIVE REPORT ─────────────────────────────────────────────────────
        public async Task<ReportListDto> GenerateNarrativeAsync(GenerateNarrativeDto dto)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("api/reports/narrative", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReportListDto>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Narrative Generation Failed: {error}");
        }

        // ── DOWNLOAD ─────────────────────────────────────────────────────────────
        public async Task<byte[]> DownloadReportAsync(int reportId)
        {
            AttachToken();
            var response = await _http.GetAsync($"api/reports/{reportId}/download");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            throw new Exception("Could not download the report file.");
        }
    }
}