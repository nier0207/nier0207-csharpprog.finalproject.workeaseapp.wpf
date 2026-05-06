using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WorkeaseAdmin_WPF.Services
{
    public class ApiService
    {
        protected readonly HttpClient _http;
        protected readonly SessionManager _session;

        public ApiService()
        {
            _session = App.Services.GetRequiredService<SessionManager>();
            _http = new HttpClient { BaseAddress = new Uri("https://localhost:7113") };
        }

        public void AttachToken()
        {
            var token = _session.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}