using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;

namespace InterviewPanelAvailabilitySystemAPI.Services.Implementation
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public ServiceResponse<ReportSlotCountDto> SlotsCountReport(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate)
        {

            var response = new ServiceResponse<ReportSlotCountDto>();
            try
            {
                var totalSlotsCount = _reportRepository.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
                if (totalSlotsCount != null)
                {
                    response.Data = totalSlotsCount;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong please try after sometime";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public ServiceResponse<IEnumerable<ReportDetailsDto>> ReportDetail(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked, int page, int pageSize)
        {
            var response = new ServiceResponse<IEnumerable<ReportDetailsDto>>();
            try
            {
                var reportData = _reportRepository.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);
                List<ReportDetailsDto> reportDtos = new List<ReportDetailsDto>();
                if (reportData != null && reportData.Any())
                {
                    foreach (var report in reportData)
                    {
                        reportDtos.Add(new ReportDetailsDto()
                        {
                            EmployeeId = report.EmployeeId,
                            TimeslotId = report.TimeslotId,
                            FirstName = report.Employee.FirstName,
                            LastName = report.Employee.LastName,
                            Email = report.Employee.Email,
                            SlotDate = report.SlotDate,
                            timeSlotName = report.Timeslot.TimeslotName
                        });
                    }
                    response.Data = reportDtos;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found!";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse<int> totalReportDetailCount(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var totalSlotsCount = _reportRepository.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);
                if (totalSlotsCount >= 0)
                {
                    response.Data = totalSlotsCount;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong please try after sometime";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }


        }
    }
}
