using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewPanelAvailabilitySystemAPI.Data.Contract
{
    public interface IRecruiterRepository
    {
      
        bool UpdateInterviewSlots(InterviewSlots interviewSlots);
        InterviewSlots? GetInterviewSlotsById(int slotId);
        bool InterviewSlotExist(int slotId, bool isBooked);
        IEnumerable<InterviewSlots> GetPaginatedInterviwerByAll(int page, int pageSize, string? searchQuery, string sortOrder, int? jobRoleId, int? roundId);
        int TotalInterviewSlotsByAll(string? searchQuery, int? jobRoleId, int? roundId);
    }
}
