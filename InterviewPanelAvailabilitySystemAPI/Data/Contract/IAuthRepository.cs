using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Data.Contract
{
    public interface IAuthRepository
    {
        Employees ValidateUser(string username);
        bool UpdateUser(Employees employee);
    }
}
