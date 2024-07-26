using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class RecruiterController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        public RecruiterController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }
        public IActionResult Index(int? jobRoleId, int? interviewRoundId, int page = 1, int pageSize = 2, string sort = "asc", string? search = "")
        {
            var apiGetInterviwerUrl = "";
            var apiGetCountUrl = "";
            try
            {
                ServiceResponse<IEnumerable<InterviewSlotsViewModel>> response = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>();
                if (jobRoleId == null && interviewRoundId == null && search == null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&sortOrder={sort}";
                }
                else if (jobRoleId != null && interviewRoundId == null && search == null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?jobRoleId={jobRoleId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&sortOrder={sort}&jobRoleId={jobRoleId}";
                }
                else if (jobRoleId == null && interviewRoundId != null && search == null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?roundId={interviewRoundId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&sortOrder={sort}&roundId={interviewRoundId}";
                }
                else if (jobRoleId != null && interviewRoundId != null && search == null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?jobRoleId={jobRoleId}&roundId={interviewRoundId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&sortOrder={sort}&jobRoleId={jobRoleId}&roundId={interviewRoundId}";
                }
                else if (jobRoleId == null && interviewRoundId == null && search != null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?searchQuery={search}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&searchQuery={search}&sortOrder={sort}";
                }
                else if (jobRoleId != null && interviewRoundId == null && search != null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?searchQuery={search}&jobRoleId={jobRoleId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&searchQuery={search}&sortOrder={sort}&jobRoleId={jobRoleId}";
                }
                else if (jobRoleId == null && interviewRoundId != null && search != null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?searchQuery={search}&roundId={interviewRoundId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&searchQuery={search}&sortOrder={sort}&roundId={interviewRoundId}";
                }
                else if (jobRoleId != null && interviewRoundId != null && search != null)
                {
                    apiGetCountUrl = $"{endPoint}Recruiter/GetTotalInterviewSlotsByAll?searchQuery={search}&jobRoleId={jobRoleId}&roundId={interviewRoundId}";
                    apiGetInterviwerUrl = $"{endPoint}Recruiter/GetPaginatedInterviwerByAll?page={page}&pageSize={pageSize}&sortOrder={sort}&jobRoleId={jobRoleId}&roundId={interviewRoundId}";
                }
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(apiGetInterviwerUrl, HttpMethod.Get, HttpContext.Request);
                var jobRoles = GetJobRoles();
                var interviewRounds = GetInterviewRounds();
                ViewBag.JobRoles = jobRoles;
                ViewBag.InterviewRound = interviewRounds;
                ViewBag.SortOrder = sort;
                ViewBag.Search = search;
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.JobRoleId = jobRoleId; // Preserve jobRoleId
                ViewBag.InterviewRoundId = interviewRoundId; // Preserve interviewRoundId



                ServiceResponse<int> countOfUser = new ServiceResponse<int>();

                countOfUser = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
                int totalCount = countOfUser.Data;
                if (totalCount == 0)
                {
                    return View(new List<InterviewSlotsViewModel>());
                }
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (page > totalPages)
                {
                    // Redirect to the first page with the new page size
                    return RedirectToAction("Index", new { page = 1, pageSize, sort = ViewBag.SortOrder, jobRoleId = ViewBag.JobRoleId, interviewRoundId = ViewBag.InterviewRoundId, search = ViewBag.Search });
                }
                ViewBag.TotalPages = totalPages;

                if (response.Success)
                {
                    return View(response.Data);
                }

                return View(new List<InterviewSlotsViewModel>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult UpdateInterviewSlot(int slotId)
        {
            try
            {
                var apiUrl = $"{endPoint}Recruiter/GetInterviewSlotsById/" + slotId;
                var response = _httpClientService.GetHttpResponseMessage<InterviewSlotsViewModel>(apiUrl, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<InterviewSlotsViewModel>>(data);
                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        InterviewSlotsViewModel viewModel = serviceResponse.Data;
                        return View(viewModel);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Timeslot selected successfully";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<InterviewSlotsViewModel>>(errorData);
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

        public IActionResult UpdateInterviewSlot(InterviewSlotsViewModel viewModel)
        {
            try
            {
                var apiUrl = $"{endPoint}Recruiter/UpdateInterviewSlot";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }
        private IEnumerable<JobRoleViewModel> GetJobRoles()
        {
            var apiUrl = $"{endPoint}Admin/GetAllJobRoles";

            ServiceResponse<IEnumerable<JobRoleViewModel>> response = new ServiceResponse<IEnumerable<JobRoleViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        private IEnumerable<InterviewRoundViewModel> GetInterviewRounds()
        {
            var apiUrl = $"{endPoint}Admin/GetAllInterviewRounds";

            ServiceResponse<IEnumerable<InterviewRoundViewModel>> response = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }
    }
}
