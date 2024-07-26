using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Data.Contract
{
    public interface IReportRepository
    {
        ReportSlotCountDto SlotsCountReport(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate);

        IEnumerable<InterviewSlots> ReportDetai(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked, int page, int pageSize);
        int totalReportDetailCount(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked);
    }
}
