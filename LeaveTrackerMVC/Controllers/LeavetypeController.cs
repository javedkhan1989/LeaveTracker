using LeaveTrackerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace LeaveTrackerMVC.Controllers
{
    public class LeavetypeController : Controller
    {
        private string LeaveTypesUrl = "https://localhost:7161/LeaveTypes";
        //  private HttpClient httpClient = new HttpClient();
        private readonly IHttpClientFactory _clientFactory;

        public LeavetypeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
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
            List<LeaveType> leaveTypes = new List<LeaveType>();
            //HttpResponseMessage response = httpClient.GetAsync(url).Result;
            var response = await client.GetAsync(LeaveTypesUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<LeaveType>>(result);
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
            return View();
        }
        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveType leavetype)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(leavetype);
            }

            try
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(leavetype);
                using var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(LeaveTypesUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                var respBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(respBody) ? "Unable to create leavetype." : respBody);
                return View(leavetype);
            }
            catch (Exception ex)
            {
                // Log exception if you have logging; for now add model error
                ModelState.AddModelError(string.Empty, "An error occurred while creating the employee.");
                return View(leavetype);
            }
        }
    }
}
