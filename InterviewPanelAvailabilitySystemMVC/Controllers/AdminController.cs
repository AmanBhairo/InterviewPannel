using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;
        public AdminController(IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler tokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _tokenHandler = tokenHandler;
        }
        public IActionResult Index(string? search, int page = 1, int pageSize = 6, string sortOrder = "asc")
        {
            var apiGetUsersUrl = "";
            var apiGetCountUrl = "";
            ViewBag.Search = search;
            try
            {
                if (search != null)
                {
                    apiGetUsersUrl = $"{endPoint}Admin/GetAllEmployees/?search={search}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                    apiGetCountUrl = $"{endPoint}Admin/GetTotalEmployeeCount/?search={search}";
                }
                else
                {
                    apiGetUsersUrl = $"{endPoint}Admin/GetAllEmployees/?page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                    apiGetCountUrl = $"{endPoint}Admin/GetTotalEmployeeCount";
                }
                ServiceResponse<int>  countOfUser = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
                int totalCount = countOfUser.Data;

                if (totalCount == 0)
                {
                    // Return an empty view
                    return View(new List<EmployeeViewModel>());
                }

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (page > totalPages)
                {
                    // Redirect to the first page with the new page size
                    return RedirectToAction("Index", new { page = 1, pageSize });
                }

                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.SortOrder = sortOrder;

                ServiceResponse<IEnumerable<EmployeeViewModel>> response = new ServiceResponse<IEnumerable<EmployeeViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(apiGetUsersUrl, HttpMethod.Get, HttpContext.Request);

                if (response != null && response.Success)
                {
                    return View(response.Data);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult AddInterviewer()
        {
            AddInterviewrViewModel addInterviewrViewModel = new AddInterviewrViewModel();
            addInterviewrViewModel.JobRoles = GetJobRoles();
            addInterviewrViewModel.InterviewRounds = GetInterviewRoundViewModels();
            return View(addInterviewrViewModel);
        }
        [HttpPost]
        public IActionResult AddInterviewer(AddInterviewrViewModel addInterviewrViewModel)
        {
            addInterviewrViewModel.JobRoles = GetJobRoles();
            addInterviewrViewModel.InterviewRounds = GetInterviewRoundViewModels();
            if (ModelState.IsValid)
            {
                try
                {
                    var apiUrl = $"{endPoint}Admin/AddEmployee";
                    var response = _httpClientService.PostHttpResponseMessage(apiUrl, addInterviewrViewModel, HttpContext.Request);

                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        return RedirectToAction("Index");
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
                            TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(addInterviewrViewModel);
        }
        private IEnumerable<JobRoleViewModel> GetJobRoles()
        {
            var apiUrl = $"{endPoint}Admin/GetAllJobRoles";

            ServiceResponse<IEnumerable<JobRoleViewModel>> response = new ServiceResponse<IEnumerable<JobRoleViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        private IEnumerable<InterviewRoundViewModel> GetInterviewRoundViewModels()
        {
            var apiUrl = $"{endPoint}Admin/GetAllInterviewRounds";

            ServiceResponse<IEnumerable<InterviewRoundViewModel>> response = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var apiUrl = $"{endPoint}Admin/GetEmployeeById/" + id;
                var response = _httpClientService.GetHttpResponseMessage<UpdateEmployeeViewModel>(apiUrl, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateEmployeeViewModel>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        IEnumerable<JobRoleViewModel> jobRoles = GetJobRoles();
                        IEnumerable<InterviewRoundViewModel> interviewRoundViewModels = GetInterviewRoundViewModels();
                        ViewBag.JobRoles = jobRoles;

                        ViewBag.interviewRoundViewModels = interviewRoundViewModels;
                        ViewBag.JobRoleId = serviceResponse.Data.JobRoleId;
                        ViewBag.InterviewRoundId = serviceResponse.Data.InterviewRoundId;
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateEmployeeViewModel>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        public IActionResult Edit(UpdateEmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var apiUrl = $"{endPoint}Admin/EditEmployee";
                    var response = _httpClientService.PutHttpResponseMessage(apiUrl, employeeViewModel, HttpContext.Request);

                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        return RedirectToAction("Index");
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
                            TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                }

            }
            IEnumerable<JobRoleViewModel> jobroles = GetJobRoles();
            IEnumerable<InterviewRoundViewModel> interviewRoundViewModels = GetInterviewRoundViewModels();
            ViewBag.JobRoles = jobroles;
            ViewBag.InterviewRoundViewModels = interviewRoundViewModels;
            return View(employeeViewModel);
        }
       
        
        [HttpPost]
        public IActionResult Delete(int employeeId)
        {
            try
            {
                var apiUrl = $"{endPoint}Admin/RemoveEmployee?id=" + employeeId;
                var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>($"{apiUrl}", HttpMethod.Put, HttpContext.Request);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = response.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
            }

            return RedirectToAction("Index");
        }
        
    }
}
