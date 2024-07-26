using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPanelAvailabilitySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("SlotsReport")]
        public IActionResult SlotsCountReport(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var response = _reportService.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ReportDetails")]
        public IActionResult ReportDetails(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked, int page =1, int pageSize = 6)
        {
            try
            {
                var response = _reportService.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TotalReportDetailCount")]
        public IActionResult TotalReportDetailCount(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked)
        {
            try
            {
                var response = _reportService.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
