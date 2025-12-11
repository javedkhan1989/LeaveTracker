using LeaveTrackerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace LeaveTrackerMVC.Controllers
{
    public class HomeController : Controller
    {
        private string url = "https://localhost:7161/employees";
        private string LeaveTypesUrl = "https://localhost:7161/LeaveTypes";
        //  private HttpClient httpClient = new HttpClient();
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
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
            List<Employee> employees = new List<Employee>();
            //HttpResponseMessage response = httpClient.GetAsync(url).Result;
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result=response.Content.ReadAsStringAsync().Result;
                var data=JsonConvert.DeserializeObject<List<Employee>>(result);
                if(data is not null)
                {
                    employees=data;
                }
            }
            return View(employees);
        }

        // GET: Home/Create
        [HttpGet]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new Employee());
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee emp)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(emp);
            }

            try
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(emp);
                using var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                var respBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(respBody) ? "Unable to create employee." : respBody);
                return View(emp);
            }
            catch (Exception ex)
            {
                // Log exception if you have logging; for now add model error
                ModelState.AddModelError(string.Empty, "An error occurred while creating the employee.");
                return View(emp);
            }
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
