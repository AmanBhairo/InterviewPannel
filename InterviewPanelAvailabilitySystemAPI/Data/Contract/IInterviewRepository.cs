using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewPanelAvailabilitySystemAPI.Data.Contract
{
    public interface IInterviewRepository
    {
        bool AddInterviewSlot(InterviewSlots slot);
        bool DeleteInterviewSlot(int id, DateTime slotDate, int timeSlotId);
        bool InterviewSlotExist(int employeeId, DateTime date, int timeslotId);
        IEnumerable<Timeslot> GetAllTimeslots();

        IEnumerable<InterviewSlots> GetAllInterviewslotsbyEmployeeId(int employeeId);
    }
}
