using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewPanelAvailabilitySystemAPI.Data.Implementation
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly IAppDbContext _context;

        public InterviewRepository(IAppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        public bool AddInterviewSlot(InterviewSlots slot)
        {
            try
            {
                var result = false;
                if (slot != null)
                {

                    _context.InterviewSlot.Add(slot);
                    _context.SaveChanges();
                    result = true;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteInterviewSlot(int id, DateTime slotDate, int timeSlotId)
        {
            try
            {
                var result = false;
                var slot = _context.InterviewSlot.FirstOrDefault(c => c.EmployeeId == id && c.SlotDate == slotDate && c.TimeslotId == timeSlotId);
                if (slot != null)
                {
                    _context.InterviewSlot.Remove(slot);
                    _context.SaveChanges();
                    result = true;
                }
                return result;
            }
            catch
            {
                  return false; 
            }
        }

        public bool InterviewSlotExist(int employeeId, DateTime date,int timeslotId)
        {
            try
            {
                var interviewSlot = _context.InterviewSlot.FirstOrDefault(c => c.EmployeeId == employeeId && c.SlotDate == date && c.TimeslotId == timeslotId);
                if (interviewSlot != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

              return false; 
            }
        }

        public IEnumerable<Timeslot> GetAllTimeslots()
        {
            try
            {
                List<Timeslot> timeslots = _context.Timeslot.ToList();
                return timeslots;
            }
            catch
            {

                 return Enumerable.Empty<Timeslot>(); 
            }
        }
        
        public IEnumerable<InterviewSlots> GetAllInterviewslotsbyEmployeeId(int employeeId)
        {
            try
            {
                List<InterviewSlots> interviewSlots = _context.InterviewSlot.
                                                       Where(c => c.EmployeeId == employeeId).ToList();
                return interviewSlots;
            }
            catch
            {
                return Enumerable.Empty<InterviewSlots>();
            }
        }
    }
}
