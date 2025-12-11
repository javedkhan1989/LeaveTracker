using LeaveTrackerMVC.Models;
using LeaveTrackerMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LeaveTrackerMVC.Controllers
{
    public class RequestController : Controller
    {
        private string LeaveRequestUrl = "https://localhost:7161/LeaveRequests";
        private string LeaveTypesUrl = "https://localhost:7161/LeaveTypes";
        //  private HttpClient httpClient = new HttpClient();
        private readonly IHttpClientFactory _clientFactory;
        private readonly LeaveApiService _apiService;

        public RequestController(IHttpClientFactory clientFactory,LeaveApiService apiService)
        {
            _clientFactory = clientFactory;
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var Role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            List<LeaveRequest> leaveTypes = new List<LeaveRequest>();
            //HttpResponseMessage response = httpClient.GetAsync(url).Result;
            var response = await client.GetAsync(LeaveRequestUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<LeaveRequest>>(result);
                if (data is not null)
                {
                    leaveTypes = data;
                }
            }

            

            return View(leaveTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadLeaveTypesFromApi();  // RELOAD dropdown if error
            return View();
        }
        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest leaverequest)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                await LoadLeaveTypesFromApi();  // RELOAD dropdown if error
                return View(leaverequest);
            }

            try
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(leaverequest);
                using var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(LeaveRequestUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                var respBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(respBody) ? "Unable to create leavetype." : respBody);
                return View(leaverequest);
            }
            catch (Exception ex)
            {
                // Log exception if you have logging; for now add model error
                ModelState.AddModelError(string.Empty, "An error occurred while creating the employee.");
                return View(leaverequest);
            }
        }

        private async Task LoadLeaveTypesFromApi()
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(LeaveTypesUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<LeaveType>>(result);

                ViewBag.LeaveTypes = data.Select(x => new SelectListItem
                {
                    Value = x.LeaveTypeId.ToString(),
                    Text = x.TypeName
                }).ToList();
            }
            else
            {
                ViewBag.LeaveTypes = new List<SelectListItem>();
            }
        }

        public async Task<IActionResult> Approve(int id)
        {
            var success = await _apiService.ApproveLeave(id);

            if (!success)
            {
                TempData["Error"] = "Unable to approve leave!";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Leave approved successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var success = await _apiService.RejectLeave(id);

            if (!success)
            {
                TempData["Error"] = "Unable to reject leave!";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Leave reject successfully!";
            return RedirectToAction("Index");
        }
    }
}
