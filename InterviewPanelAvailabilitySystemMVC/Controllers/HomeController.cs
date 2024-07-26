using InterviewPanelAvailabilitySystemMVC.Implementation;
using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.Models;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler tokenHandler)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _tokenHandler = tokenHandler;
        }

        public IActionResult Index()
        {
            try
            {
                var firstName = "";
                if (User.Identity.IsAuthenticated)
                {
                    var id = Convert.ToInt32(User.FindFirst("EmployeeId").Value);
                    if (id > 0)
                    {
                        var apiUrl = $"{endPoint}Admin/GetEmployeeById/" + id;
                        var response = _httpClientService.GetHttpResponseMessage<EmployeeViewModel>(apiUrl, HttpContext.Request);

                        if (response.IsSuccessStatusCode)
                        {
                            string data1 = response.Content.ReadAsStringAsync().Result;
                            var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<EmployeeViewModel>>(data1);

                            if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                            {
                                firstName = serviceResponse.Data.FirstName;
                                ViewBag.FirstName = firstName;
                                return View();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving employee details. Please try again later.";
                return View();
            }
            return View();
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
