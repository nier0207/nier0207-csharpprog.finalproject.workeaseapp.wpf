using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class ChildService : ApiService
    {
        public ChildService() : base() { }

        public async Task<List<ChildSummaryDto>?> GetAllChildrenAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<ChildSummaryDto>>("/api/Children");
        }

        public async Task<ChildSummaryDto?> GetChildByIdAsync(int childId)
        {
            AttachToken();
            return await _http.GetFromJsonAsync<ChildSummaryDto>($"/api/Children/{childId}");
        }

        public async Task<Child> CreateChildWithParentAsync(CreateChildDto newChild)
        {
            AttachToken();
            var response = await _http.PostAsJsonAsync("/api/Children", newChild);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Child>();
            }
            return null;
        }

        public async Task<bool> UpdateChildAsync(int childId, UpdateChildDto updatedChild)
        {
            AttachToken();
            var response = await _http.PutAsJsonAsync($"/api/Children/{childId}", updatedChild);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LinkParentAsync(int childId, int parentId)
        {
            AttachToken();
            try
            {
                var response = await _http.PutAsync(
                    $"/api/children/{childId}/link-parent/{parentId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(
                        $"[WPF] LinkParent error: {error}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[WPF] LinkParent exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UnlinkParentAsync(int childId)
        {
            AttachToken();
            try
            {
                var response = await _http.PutAsync(
                    $"/api/children/{childId}/unlink-parent", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[WPF] UnlinkParent exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteChildAsync(int childId)
        {
            AttachToken();
            var response = await _http.DeleteAsync($"/api/Children/{childId}");
            return response.IsSuccessStatusCode;
        }
    }
}
