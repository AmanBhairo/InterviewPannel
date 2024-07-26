using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;
        public AuthController(IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler tokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _tokenHandler = tokenHandler;
        }
        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginUser(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string apiUrl = $"{endPoint}Auth/Login";
                    var response = _httpClientService.PostHttpResponseMessage(apiUrl, login, HttpContext.Request);
                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                        string token = serviceResponse.Data;
                        Response.Cookies.Append("jwtToken", token, new CookieOptions // Set the JWT token cookie
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.UtcNow.AddHours(1)
                        });

                        var jwtToken = _tokenHandler.ReadJwtToken(token);
                        var employeeId = jwtToken.Claims.First(claim => claim.Type == "EmployeeId").Value;
                        Response.Cookies.Append("employeeId", employeeId, new CookieOptions // Set the employeeId cookie
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.UtcNow.AddDays(1),
                        });

                        int id = Convert.ToInt32(employeeId);
                        if (id > 0)
                        {
                            try
                            {
                                var apiUrl1 = $"{endPoint}Admin/GetEmployeeById/" + id;
                                var response1 = _httpClientService.GetHttpResponseMessage<EmployeeViewModel>(apiUrl1, HttpContext.Request);

                                if (response1.IsSuccessStatusCode)
                                {
                                    string data1 = response1.Content.ReadAsStringAsync().Result;
                                    var serviceResponse1 = JsonConvert.DeserializeObject<ServiceResponse<EmployeeViewModel>>(data1);

                                    if (serviceResponse1 != null && serviceResponse1.Success && serviceResponse1.Data != null && serviceResponse1.Data.ChangePassword)
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("ChangePassword", "Auth");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TempData["ErrorMessage"] = "An error occurred while retrieving employee details. Please try again later.";
                            }
                        }
                    }
                    else
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                        if (serviceResponse != null)
                        {
                            TempData["ErrorMessage"] = serviceResponse?.Message;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Something went wrong, please try after sometime";
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    TempData["ErrorMessage"] = "An error occurred while communicating with the server. Please try again later.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                }
            }
            return View(login);
        }

        public IActionResult LogoutUser()
        {
            Response.Cookies.Delete("jwtToken");
            Response.Cookies.Delete("employeeId");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var apiUrl = $"{endPoint}Auth/ChangePassword";
                    HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, changeViewModel, HttpContext.Request);

                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        //User.Identity.Name = userViewModel.LoginId;
                        Response.Cookies.Delete("jwtToken");
                        return RedirectToAction("LoginUser", "Auth");
                    }
                    else
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                        if (serviceResponse != null)
                        {
                            TempData["ErrorMessage"] = serviceResponse?.Message;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Something went wrong, please try after sometime";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                }
            }
            return View(changeViewModel);
        }
    }
}
