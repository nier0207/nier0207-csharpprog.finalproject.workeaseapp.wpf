using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Services
{
    public class AutoFeeService : ApiService
    {
        public AutoFeeService() : base() { }

        public async Task<bool> GenerateMonthlyFeesAsync()
        {
            AttachToken();
            var response = await _http.PostAsync("/api/fees/generate-monthly", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ProcessOverdueFeesAsync()
        {
            AttachToken();
            var response = await _http.PostAsync("/api/fees/process-overdue", null);
            return response.IsSuccessStatusCode;
        }
    }
}
