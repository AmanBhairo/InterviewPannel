using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace InterviewPanelAvailabilitySystemAPI.Data.Implementation
{
    public class ReportRepository : IReportRepository
    {
        private readonly IAppDbContext _appDbContext;
        public ReportRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ReportSlotCountDto SlotsCountReport(int? jobRoleId,int? interViewRoundId,DateTime? startDate, DateTime? endDate)
        {
            try
            {
                ReportSlotCountDto report = new ReportSlotCountDto();

                if (jobRoleId != null)
                {
                    var availableSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == false && s.Employee.JobRoleId == jobRoleId && s.Employee.IsActive);
                    var bookedSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == true && s.Employee.JobRoleId == jobRoleId && s.Employee.IsActive);
                    report.AvailableSlot = availableSlot.Count();
                    report.BookedSlot = bookedSlot.Count();
                    return report;
                }
                else if (interViewRoundId != null)
                {
                    var availableSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == false && s.Employee.InterviewRoundId == interViewRoundId && s.Employee.IsActive);
                    var bookedSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == true && s.Employee.InterviewRoundId == interViewRoundId && s.Employee.IsActive);
                    report.AvailableSlot = availableSlot.Count();
                    report.BookedSlot = bookedSlot.Count();
                    return report;
                }
                else
                {
                    var availableSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == false && s.SlotDate >= startDate && s.SlotDate <= endDate && s.Employee.IsActive);
                    var bookedSlot = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(s => s.IsBooked == true && s.SlotDate >= startDate && s.SlotDate <= endDate && s.Employee.IsActive);
                    report.AvailableSlot = availableSlot.Count();
                    report.BookedSlot = bookedSlot.Count();
                    return report;
                }
            }
            catch
            {
                ReportSlotCountDto report = new ReportSlotCountDto();
                return report;
            }
        }


        public IEnumerable<InterviewSlots> ReportDetai(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate,bool booked, int page, int pageSize)
        {

            List<InterviewSlots> report = new List<InterviewSlots>();
            try
            {
                int skip = (page - 1) * pageSize;

                if (jobRoleId != null)
                {
                    report = _appDbContext.InterviewSlot.Include(s => s.Employee).Include(i => i.Timeslot).Where(c => c.Employee.JobRoleId == jobRoleId && c.IsBooked == booked && c.Employee.IsActive).Skip(skip)
                            .Take(pageSize).ToList();
                }
                else if (interViewRoundId != null)
                {
                    report = _appDbContext.InterviewSlot.Include(s => s.Employee).Include(i => i.Timeslot).Where(c => c.Employee.InterviewRoundId == interViewRoundId && c.IsBooked == booked && c.Employee.IsActive).Skip(skip)
                            .Take(pageSize).ToList();
                }
                else
                {
                    report = _appDbContext.InterviewSlot.Include(s => s.Employee).Include(i => i.Timeslot).Where(c => c.IsBooked == booked && c.SlotDate >= startDate && c.SlotDate <= endDate && c.Employee.IsActive).Skip(skip)
                            .Take(pageSize).ToList();
                }
                return report;
            }
            catch
            {
                return report;
            }
        }

        public int totalReportDetailCount(int? jobRoleId, int? interViewRoundId, DateTime? startDate, DateTime? endDate, bool booked)
        {
            try
            {
                int total = 0;
                if (jobRoleId != null)
                {
                    total = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(c => c.Employee.JobRoleId == jobRoleId && c.IsBooked == booked && c.Employee.IsActive).Count();
                }
                else if (interViewRoundId != null)
                {
                    total = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(c => c.Employee.InterviewRoundId == interViewRoundId && c.IsBooked == booked && c.Employee.IsActive).Count();
                }
                else
                {
                    total = _appDbContext.InterviewSlot.Include(s => s.Employee).Where(c => c.IsBooked == booked && c.SlotDate >= startDate && c.SlotDate <= endDate && c.Employee.IsActive).Count();
                }
                return total;
            }
            catch
            {
                return 0;
            }
        }
    }
}
