using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewPanelAvailabilitySystemAPI.Data.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AdminRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Employees> GetAllEmployees(int page, int pageSize, string? search, string sortOrder)
        {
            try
            {
                int skip = (page - 1) * pageSize;
                IQueryable<Employees> query = _appDbContext.Employee.Include(c => c.JobRole).Include(c => c.InterviewRound).Where(c => !c.IsAdmin && c.IsActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search) || c.Email.Contains(search));
                }

                switch (sortOrder.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(c => c.FirstName).ThenBy(c => c.Email);
                        break;
                    case "desc":
                        query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.Email);
                        break;
                    default:
                        query = query.OrderBy(c => c.FirstName);
                        break;
                }
                return query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return Enumerable.Empty<Employees>();
            }
        }

        //UpdateEmployee
        public bool UpdateEmployee(Employees employees)
        {
            var result = false;
            try
            {
                if (employees != null)
                {
                    _appDbContext.Employee.Update(employees);
                    _appDbContext.SaveChanges();
                    result = true;
                }

                return result;
            }
            catch
            {
                result = false;
                return result;
            }
        }


        public Employees GetEmployeeByEmployeeIdAndEmail(int employeeId, string email)
        {
            try
            {
                var employees = _appDbContext.Employee.FirstOrDefault(c => c.EmployeeId != employeeId
                && c.Email == email && c.IsActive);
                return employees;
            }
            catch
            {
                var employeeCatch = new Employees();
                return employeeCatch;
            }
        }

        //GetEmployeeById
        public Employees? GetEmployeeById(int id)
        {
            try
            {
                var employees = _appDbContext.Employee.Include(p => p.JobRole).Include(p => p.InterviewRound).FirstOrDefault(c => c.EmployeeId == id);
                //if(employees.IsAdmin)
                //{
                //    return new Employees();
                //}
                return employees;
            }
            catch
            {
                return null;
            }
        }
        public int TotalEmployeeCount(string? search)
        {
            try
            {
                IQueryable<Employees> query = _appDbContext.Employee.Where(c => !c.IsAdmin && c.IsActive);
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search) || c.Email.Contains(search));
                }
                return query.Count();
            }
            catch 
            { 
                return 0; 
            }
        }
        public bool EmployeeExists(string email)
        {
            try
            {
                if (_appDbContext.Employee.Any(c => c.Email.ToLower() == email.ToLower() && c.IsActive))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        public bool AddEmployee(Employees employee)
        {
            try
            {
                var result = false;
                if (employee != null)
                {
                    _appDbContext.Employee.Add(employee);
                    _appDbContext.SaveChanges();
                    return true;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<JobRole> GetAllJobRoles()
        {
            try
            {
                List<JobRole> jobRoles = _appDbContext.JobRole.ToList();
                return jobRoles;
            }
            catch
            {
                return Enumerable.Empty<JobRole>();
            }
        }
        public IEnumerable<InterviewRounds> GetAllInterviewRounds()
        {
            try
            {
                List<InterviewRounds> interviewRounds = _appDbContext.InterviewRound.ToList();
                return interviewRounds;
            }
            catch
            {
                return Enumerable.Empty<InterviewRounds>();
            }
        }
    }
}
