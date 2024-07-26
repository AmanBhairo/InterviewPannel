using InterviewPanelAvailabilitySystemAPI.Dtos;

namespace InterviewPanelAvailabilitySystemAPI.Services.Contract
{
    public interface IReportService
    {
        ServiceResponse<ReportSlotCountDto> SlotsCountReport(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate);
        ServiceResponse<IEnumerable<ReportDetailsDto>> ReportDetail(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked, int page, int pageSize);
        ServiceResponse<int> totalReportDetailCount(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked);
    }
}
