using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Data.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AuthRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Employees ValidateUser(string username)
        {
            try
            {
                Employees? user = _appDbContext.Employee.FirstOrDefault(c => c.Email == username.ToLower());
                return user;
            }
            catch 
            {
                var employeeCatch = new Employees();
                return employeeCatch;
            }
        }
        public bool UpdateUser(Employees employee)
        {
            try
            {
                var result = false;
                if (employee != null)
                {
                    _appDbContext.Employee.Update(employee);
                    _appDbContext.SaveChanges();
                    result = true;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
