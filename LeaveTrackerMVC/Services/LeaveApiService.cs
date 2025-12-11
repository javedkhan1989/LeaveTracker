using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LeaveTrackerMVC.Services
{
    public class LeaveApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string baseUrl = "https://localhost:7161/LeaveRequests/";

        public LeaveApiService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ApproveLeave(int id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return false;

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var url = $"{baseUrl}{id}/approve";

            var response = await client.PutAsync(url, null); // No body required

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RejectLeave(int id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return false;

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var url = $"{baseUrl}{id}/reject";

            var response = await client.PutAsync(url, null); // No body required

            return response.IsSuccessStatusCode;
        }
    }
}
