using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;


        private string endPoint;

        public ReportController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];

        }


        public IActionResult DetailedReport(int? jobRoleId, bool booked = false, int page = 1, int pageSize = 4)
        {
            try
            {
                List<JobRoleViewModel> JobRoles = new List<JobRoleViewModel>();
                JobRoles = GetJobRole();
                ViewBag.JobRoles = JobRoles;

                var apiUrl = $"{endPoint}Report/ReportDetails"
                    + "?jobRoleId=" + jobRoleId
                    + "&booked=" + booked
                    + "&page=" + page
                    + "&pageSize=" + pageSize;

                var totalCountApiUrl = $"{endPoint}Report/TotalReportDetailCount"
                    + "?jobRoleId=" + jobRoleId
                    + "&booked=" + booked;


                ServiceResponse<int> countResponse = new ServiceResponse<int>();

                countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                    (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

                var totalCount = countResponse.Data;

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (totalPages != 0)
                {
                    if (page > totalPages)
                    {

                        return RedirectToAction("DetailedReport", new { jobRoleId, booked, page = totalPages, pageSize });
                    }
                }


                ViewBag.JobRoleId = jobRoleId;
                //ViewBag.InterViewRoleId = interViewRoundId;
                //ViewBag.StartDate = startDate;
                //ViewBag.EndDate = endDate;
                ViewBag.Booked = booked;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.TotalPages = totalPages;

                ServiceResponse<IEnumerable<DetailedReportViewModel>> reportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    if(jobRoleId != null && jobRoleId != 0)
                    {
                        TempData["ErrorMessage"] = "No record found";
                    }
                    return View(new List<DetailedReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult DetailedInterviewRoundReport(int? interViewRoundId, bool booked = false, int page = 1, int pageSize = 4)
            {
            try
            {
                
                List<InterviewRoundViewModel> InterviewRound = new List<InterviewRoundViewModel>();
                InterviewRound = GetInterviewRound();
                ViewBag.InterviewRounds = InterviewRound;

                var apiUrl = $"{endPoint}Report/ReportDetails"
                    + "?interViewRoundId=" + interViewRoundId
                    + "&booked=" + booked
                    + "&page=" + page
                    + "&pageSize=" + pageSize;

                var totalCountApiUrl = $"{endPoint}Report/TotalReportDetailCount"
                    + "?interViewRoundId=" + interViewRoundId
                    + "&booked=" + booked;


                ServiceResponse<int> countResponse = new ServiceResponse<int>();

                countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                    (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

                var totalCount = countResponse.Data;

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (totalPages != 0)
                {
                    if (page > totalPages)
                    {

                    return RedirectToAction("DetailedInterviewRoundReport", new { interViewRoundId, booked, page = totalPages, pageSize });
                }
            }


                ViewBag.InterViewRoundId = interViewRoundId;
                //ViewBag.StartDate = startDate;
                //ViewBag.EndDate = endDate;
                ViewBag.Booked = booked;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.TotalPages = totalPages;

                ServiceResponse<IEnumerable<DetailedReportViewModel>> reportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    if(interViewRoundId != null && interViewRoundId != 0)
                    {
                        TempData["ErrorMessage"] = "No record found";
                    }
                    return View(new List<DetailedReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }

        }


        public IActionResult DetailedDateRangeBasedReport(string? startDate, string? endDate, bool booked = false, int page = 1, int pageSize = 4)
        {
            try
            {
                if(startDate == null || endDate == null)
                {
                    ViewBag.StartDate = startDate;
                    ViewBag.EndDate = endDate;
                    ViewBag.Booked = booked;
                    ViewBag.page = 1;
                    ViewBag.pageSize = pageSize;
                    return View(new List<DetailedReportViewModel>());
                }
                var apiUrl = $"{endPoint}Report/ReportDetails"
                + "?startDate=" + startDate
                + "&endDate=" + endDate
                + "&booked=" + booked
                + "&page=" + page
                + "&pageSize=" + pageSize;

                var totalCountApiUrl = $"{endPoint}Report/TotalReportDetailCount"
                    + "?startDate=" + startDate
                    + "&endDate=" + endDate
                    + "&booked=" + booked;


                ServiceResponse<int> countResponse = new ServiceResponse<int>();

                countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                    (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

                var totalCount = countResponse.Data;

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (totalPages != 0)
                {
                    if (page > totalPages)
                    {

                    return RedirectToAction("DetailedDateRangeBasedReport", new { startDate, endDate, booked, page = totalPages, pageSize });
                }
            }



                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
                ViewBag.Booked = booked;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.TotalPages = totalPages;

                ServiceResponse<IEnumerable<DetailedReportViewModel>> reportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    if(startDate != null && endDate != null)
                    {
                        TempData["ErrorMessage"] = "No record found";
                    }
                    return View(new List<DetailedReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult JobRoleBasedCountReportReport(int? jobRoleId)
        {
            try
            {
                List<JobRoleViewModel> JobRoles = new List<JobRoleViewModel>();
                JobRoles = GetJobRole();
                ViewBag.JobRoles = JobRoles;
                ViewBag.JobRoleId = jobRoleId;


                var apiUrl = $"{endPoint}Report/SlotsReport"
                    + "?jobRoleId=" + jobRoleId;


                ServiceResponse<SlotCountReportViewModel> reportResponse = new ServiceResponse<SlotCountReportViewModel>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "No record found";
                    return View(new List<SlotCountReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult InterviewRoundBasedCountReportReport(int? interviewRoundId)
        {
            try
            {
                List<InterviewRoundViewModel> InterviewRounds = new List<InterviewRoundViewModel>();
                InterviewRounds = GetInterviewRound();
                ViewBag.InterviewRounds = InterviewRounds;
                ViewBag.InterviewRoundId = interviewRoundId;


                var apiUrl = $"{endPoint}Report/SlotsReport"
                    + "?interViewRoundId=" + interviewRoundId;


                ServiceResponse<SlotCountReportViewModel> reportResponse = new ServiceResponse<SlotCountReportViewModel>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "No record found";
                    return View(new List<SlotCountReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult DateRangeBasedCountReportReport(string? startDate, string? endDate)
        {
            try
            {
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
                var apiUrl = $"{endPoint}Report/SlotsReport"
                    + "?startDate=" + startDate
                    + "&endDate=" + endDate;


                ServiceResponse<SlotCountReportViewModel> reportResponse = new ServiceResponse<SlotCountReportViewModel>();
                reportResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>
                    (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (reportResponse.Success)
                {

                    return View(reportResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "No record found";
                    return View(new List<SlotCountReportViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        private List<JobRoleViewModel> GetJobRole()
        {
            ServiceResponse<IEnumerable<JobRoleViewModel>> response = new ServiceResponse<IEnumerable<JobRoleViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>
                ($"{endPoint}Admin/GetAllJobRoles", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<JobRoleViewModel>();
        }

        private List<InterviewRoundViewModel> GetInterviewRound()
        {
            ServiceResponse<IEnumerable<InterviewRoundViewModel>> response = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>
                ($"{endPoint}Admin/GetAllInterviewRounds", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<InterviewRoundViewModel>();
        }
    }
}
