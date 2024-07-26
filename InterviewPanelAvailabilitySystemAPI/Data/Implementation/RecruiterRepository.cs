using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InterviewPanelAvailabilitySystemAPI.Data.Implementation
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly IAppDbContext _context;

        public RecruiterRepository(IAppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        public bool InterviewSlotExist(int slotId, bool isBooked)
        {
            try
            {
                var interviewSlots = _context.InterviewSlot.FirstOrDefault(c => c.SlotId == slotId && c.IsBooked == true);
                if (interviewSlots != null)
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
        public bool UpdateInterviewSlots(InterviewSlots interviewSlots)
        {
            try
            {
                var result = false;
                if (interviewSlots != null)
                {
                    _context.InterviewSlot.Update(interviewSlots);
                    _context.InterviewSlot.Where(c => c.IsBooked == false);
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
        public InterviewSlots? GetInterviewSlotsById(int slotId)
        {
            try
            {
                var interviewSlots = _context.InterviewSlot.Include(c => c.Employee).Include(c => c.Timeslot).Include(c => c.Employee.JobRole).Include(c => c.Employee.InterviewRound).FirstOrDefault(c => c.SlotId == slotId);
                if (interviewSlots != null)
                {
                    var slotDate = interviewSlots.SlotDate;
                    var employeeId = interviewSlots.EmployeeId;
                    var timeslotId = interviewSlots.TimeslotId;
                }

                return interviewSlots;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<InterviewSlots> GetPaginatedInterviwerByAll(int page, int pageSize, string? searchQuery, string sortOrder, int? jobRoleId, int? roundId)
        {
            try
            {
                int skip = (page - 1) * pageSize;
                IQueryable<InterviewSlots> query = _context.InterviewSlot
                     .Include(c => c.Timeslot).Include(c => c.Employee).Include(c => c.Employee.JobRole).Include(c => c.Employee.InterviewRound)
                     .Where(c => c.IsBooked == false)
                     .Where(c => c.Employee.IsActive);
                if (jobRoleId != null && roundId != null)
                {
                    query = query
                        .Where(c => c.Employee.InterviewRoundId == roundId)
                        .Where(c => c.Employee.JobRoleId == jobRoleId);
                }
                else if (jobRoleId == null && roundId != null)
                {
                    query = query
                     .Where(c => c.Employee.InterviewRoundId == roundId);
                }
                else if (jobRoleId != null && roundId == null)
                {
                    query = query

                     .Where(c => c.Employee.JobRoleId == jobRoleId);
                }




                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(c => c.Employee.FirstName.Contains(searchQuery) || c.Employee.LastName.Contains(searchQuery));
                }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.Employee.FirstName).ThenBy(c => c.Employee.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.Employee.FirstName).ThenByDescending(c => c.Employee.LastName);
                    break;
                default:
                    query = query.OrderBy(c => c.Employee.FirstName).ThenBy(c => c.Employee.LastName);
                    break;
            }

                return query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return Enumerable.Empty<InterviewSlots>();
            }
        }

        public int TotalInterviewSlotsByAll(string? searchQuery, int? jobRoleId, int? roundId)
        {
            try
            {


                IQueryable<InterviewSlots> query = _context.InterviewSlot.Include(C => C.Employee).Where(c => c.IsBooked == false).Where(c => c.Employee.IsActive);
            if (jobRoleId != null && roundId != null)
            {
                query = query
                    .Where(c => c.Employee.InterviewRoundId == roundId)
                    .Where(c => c.Employee.JobRoleId == jobRoleId);
            }
            else if (jobRoleId == null && roundId != null)
            {
                query = query
                 .Where(c => c.Employee.InterviewRoundId == roundId);
            }
            else if (jobRoleId != null && roundId == null)
            {
                query = query

                     .Where(c => c.Employee.JobRoleId == jobRoleId);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(c => c.Employee.FirstName.Contains(searchQuery) || c.Employee.LastName.Contains(searchQuery));
                }

                return query.Count();
            }
            catch
            {
                return 0;
            }
        }

    }
}
