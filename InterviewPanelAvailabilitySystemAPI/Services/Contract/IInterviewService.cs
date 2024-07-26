using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Services.Infrastructure
{
    public interface IInterviewService
    {
        ServiceResponse<string> AddInterviewSlot(InterviewSlots slots);

       ServiceResponse<string> RemoveSlot(int id, DateTime slotDate, int timeSlotId);

        ServiceResponse<IEnumerable<TimeslotDto>> GetAllTimeslots();

        ServiceResponse<IEnumerable<InterviewSlotsDto>> GetAllInterviewslotsbyEmployeeId(int employeeId);
    }
}
