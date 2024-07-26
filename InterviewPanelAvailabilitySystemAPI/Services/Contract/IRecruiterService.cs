using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Services.Contract
{
    public interface IRecruiterService
    {

        ServiceResponse<InterviewSlotsDto> GetInterviewSlotsById(int slotId);
        ServiceResponse<string> ModifyInterviewSlots(InterviewSlots slots);

        ServiceResponse<IEnumerable<InterviewSlotsDto>> GetPaginatedInterviwerByAll(int page, int pageSize, string? searchQuery, string sortOrder, int? jobRoleId, int? roundId);

        ServiceResponse<int> TotalInterviewSlotsByAll(string? searchQuery, int? jobRoleId, int? roundId);
    }
}
