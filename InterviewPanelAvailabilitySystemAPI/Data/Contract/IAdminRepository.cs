using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Data.Contract
{
    public interface IAdminRepository
    {
        IEnumerable<Employees> GetAllEmployees(int page, int pageSize, string? search, string sortOrder);


        //bool DeleteEmployee(DeleteEmployeeDto deleteEmployee);
        bool UpdateEmployee(Employees employees);
        Employees GetEmployeeByEmployeeIdAndEmail(int employeeId, string email);

        Employees GetEmployeeById(int id);
        bool EmployeeExists(string email);
        bool AddEmployee(Employees employee);
        int TotalEmployeeCount(string? search);
        IEnumerable<JobRole> GetAllJobRoles();
        IEnumerable<InterviewRounds> GetAllInterviewRounds();
    }
}
