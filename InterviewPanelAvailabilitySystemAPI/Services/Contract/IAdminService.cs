using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Services.Contract
{
    public interface IAdminService
    {
        ServiceResponse<IEnumerable<EmployeesDto>> GetAllEmployees(int page, int pageSize, string? search, string sortOrder);


        ServiceResponse<string> DeleteEmployee(int id);

        ServiceResponse<string> UpdateEmployee(UpdateEmployeeDtos employees);

        ServiceResponse<EmployeesDto> GetEmployeeById(int id);
        ServiceResponse<string> AddEmployee(AddEmployeeDto register);
        ServiceResponse<int> TotalEmployeeCount(string? search);
        ServiceResponse<IEnumerable<JobRoleDto>> GetAllJobRoles();
        ServiceResponse<IEnumerable<InterviewRoundsDto>> GetAllInterviewRounds();
        ServiceResponse<bool> GetIsChangedPasswordById(int id);

    }
}
